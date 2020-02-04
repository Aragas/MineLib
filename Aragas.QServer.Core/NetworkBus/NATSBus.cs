using Aragas.QServer.Core.Extensions;

using NATS.Client;
using NATS.Client.Rx;

using Nito.AsyncEx;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Core.NetworkBus
{
    [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly")]
    public class NATSBus : BaseNATSNetworkBus, INetworkBus
    {
        protected IConnection _connection;

        public NATSBus(Options options)
        {
            _connection = new ConnectionFactory().CreateConnection(options);
        }
        public NATSBus()
        {
            _connection = new ConnectionFactory().CreateConnection(ConnectionFactory.GetDefaultOptions().SetDefaultArgs());
        }

        public void Publish<TMessage>(TMessage message, Guid? referenceId = null) where TMessage : notnull, IMessage =>
            _connection.Publish(GetSubject(message, referenceId), message.GetData().ToArray());
        TMessageResponse INetworkBus.PublishAndWaitForReply<TMessageRequest, TMessageResponse>(TMessageRequest message, Guid? referenceId, int timeout)
        {
            using var cancellationTokenSource = new CancellationTokenSource(timeout);
            var returnLock = new TaskCompletionSource<TMessageResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
            using var sub = _connection.SubscribeAsync(GetSubject<TMessageResponse>(referenceId), (s, e) =>
            {
                var response = new TMessageResponse();
                response.SetData(e.Message.Data);
                returnLock.SetResult(response);
            });
            Publish(message, referenceId);

            return returnLock.Task.WaitAsync(cancellationTokenSource.Token).GetAwaiter().GetResult();
        }
        IEnumerable<TMessageResponse> INetworkBus.PublishAndWaitForReplyEnumerable<TMessageRequest, TMessageResponse>(TMessageRequest message, Guid? referenceId, int timeout)
        {
            var observable = _connection.Observe(GetSubject<TMessageResponse>(referenceId))
                .Select(msg =>
                {
                    var response = new TMessageResponse();
                    response.SetData(msg.Data);
                    return response;
                })
                .Timeout(TimeSpan.FromMilliseconds(timeout == -1 ? int.MaxValue : timeout))
                .Catch<TMessageResponse, TimeoutException>(_ => Observable.Empty<TMessageResponse>());
            using var _ = observable.Subscribe();
            Publish(message, referenceId);

            foreach (var response in observable.ToEnumerable())
            {
                yield return response;

                if (response.IsLastMessage)
                    yield break;
            }
        }

        IDisposable INetworkBus.Subscribe<TMessage>(Action<TMessage> func, Guid? referenceId) =>
            _connection.SubscribeAsync(GetSubject<TMessage>(referenceId), (s, e) =>
            {
                var request = new TMessage();
                request.SetData(e.Message.Data);
                func(request);
            });
        IDisposable INetworkBus.SubscribeAndReply<TMessageRequest>(Func<TMessageRequest, IMessage> func, Guid? referenceId) =>
            _connection.SubscribeAsync(GetSubject<TMessageRequest>(referenceId), (s, e) =>
            {
                var request = new TMessageRequest();
                request.SetData(e.Message.Data);
                var response = func(request);

                Publish(response, referenceId);
            });
        IDisposable INetworkBus.SubscribeAndReplyEnumerable<TMessageRequest, TMessageResponse>(Func<TMessageRequest, IEnumerable<TMessageResponse>> func, Guid? referenceId) =>
            _connection.SubscribeAsync(GetSubject<TMessageRequest>(), (s, e) =>
            {
                var request = new TMessageRequest();
                request.SetData(e.Message.Data);
                foreach (var response in func(request))
                    Publish(response, referenceId);
            });

        public void Dispose()
        {
            _connection.Dispose();
        }
    }

    public sealed class AsyncNATSBus : NATSBus, IAsyncNetworkBus
    {
        public AsyncNATSBus(Options options) : base(options) { }
        public AsyncNATSBus() : base() { }

        public Task PublishAsync<TMessage>(TMessage message, Guid? referenceId) where TMessage : notnull, IMessage =>
            Task.Run(() => _connection.Publish(GetSubject(message, referenceId), message.GetData().ToArray()));
        async Task<TMessageResponse> IAsyncNetworkBus.PublishAndWaitForReplyAsync<TMessageRequest, TMessageResponse>(TMessageRequest message, Guid? referenceId, int timeout)
        {
            using var cancellationTokenSource = new CancellationTokenSource(timeout);
            var returnLock = new TaskCompletionSource<TMessageResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
            using var sub = _connection.SubscribeAsync(GetSubject<TMessageResponse>(referenceId), (s, e) =>
            {
                // TODO:
                try
                {
                    var response = new TMessageResponse();
                    response.SetData(e.Message.Data);
                    returnLock.SetResult(response);
                }
                catch (Exception) { }

            });
            await PublishAsync(message, referenceId);

            return await returnLock.Task.WaitAsync(cancellationTokenSource.Token);
        }
        async IAsyncEnumerable<TMessageResponse> IAsyncNetworkBus.PublishAndWaitForReplyEnumerableAsync<TMessageRequest, TMessageResponse>(TMessageRequest message, Guid? referenceId, int timeout)
        {
            var observable = _connection.Observe(GetSubject<TMessageResponse>(referenceId))
                .Select(msg =>
                {
                    var response = new TMessageResponse();
                    response.SetData(msg.Data);
                    return response;
                })
                .Timeout(TimeSpan.FromMilliseconds(timeout == -1 ? int.MaxValue : timeout))
                .Catch<TMessageResponse, TimeoutException>(_ => Observable.Empty<TMessageResponse>());
            using var _ = observable.Subscribe();
            await PublishAsync(message, referenceId);

            await foreach (var response in observable.ToAsyncEnumerable())
            {
                 yield return response;

                if (response.IsLastMessage)
                    yield break;
            }
        }

        Task<IDisposable> IAsyncNetworkBus.SubscribeAsync<TMessage>(Func<TMessage, Task> func, Guid? referenceId) =>
            Task.FromResult((IDisposable) _connection.SubscribeAsync(GetSubject<TMessage>(referenceId), async (s, e) =>
            {
                var request = new TMessage();
                request.SetData(e.Message.Data);
                await func(request);
            }));
        Task<IDisposable> IAsyncNetworkBus.SubscribeAndReplyAsync<TMessageRequest>(Func<TMessageRequest, Task<IMessage>> func, Guid? referenceId) =>
            Task.FromResult((IDisposable) _connection.SubscribeAsync(GetSubject<TMessageRequest>(referenceId), async (s, e) =>
            {
                var request = new TMessageRequest();
                request.SetData(e.Message.Data);
                var response = await func(request);
                await PublishAsync(response, referenceId);
            }));
        Task<IDisposable> IAsyncNetworkBus.SubscribeAndReplyEnumerableAsync<TMessageRequest, TMessageResponse>(Func<TMessageRequest, IAsyncEnumerable<TMessageResponse>> func, Guid? referenceId) =>
            Task.FromResult((IDisposable) _connection.SubscribeAsync(GetSubject<TMessageRequest>(), async (s, e) =>
            {
                var request = new TMessageRequest();
                request.SetData(e.Message.Data);
                await foreach (var response in func(request))
                    await PublishAsync(response, referenceId);
            }));
    }
}