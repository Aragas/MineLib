/*
using NATS.Client;
using NATS.Client.Rx;

using Nito.AsyncEx;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Core.NetworkBus
{
    public static class IConnectionMessageExtensions
    {
        public static void Publish(this IConnection conn, IMessage message, Guid? referenceId = null)
        {
            var subject = message.Name;
            if (referenceId != null)
                subject += $"-{referenceId}";

            conn.Publish(subject.ToLowerInvariant(), message.GetData().ToArray());
        }
        public static TMessageResponse? PublishAndWaitForReply<TMessageRequest, TMessageResponse>(this IConnection connection, TMessageRequest message, Guid? referenceId = null, int timeout = -1)
            where TMessageRequest : notnull, IMessage
            where TMessageResponse : class, IMessage, new()
        {
            var subject = new TMessageResponse().Name;
            if (referenceId != null)
                subject += $"-{referenceId}";

            var cancellationTokenSource = new CancellationTokenSource(timeout);
            var returnLock = new TaskCompletionSource<TMessageResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
            using var sub = connection.SubscribeAsync(subject.ToLowerInvariant(), (s, e) =>
            {
                var response = new TMessageResponse();
                response.SetData(e.Message.Data);
                returnLock.SetResult(response);
            });
            connection.Publish(message, referenceId);

            return returnLock.Task.WaitAsync(cancellationTokenSource.Token).GetAwaiter().GetResult();
        }
        public static IEnumerable<TMessageResponse> PublishAndWaitForReplyEnumerable<TMessageRequest, TMessageResponse>(this IConnection connection, TMessageRequest message, Guid? referenceId = null, int timeout = -1)
            where TMessageRequest : notnull, IMessage
            where TMessageResponse : class, IEnumerableMessage, new()
        {
            var subject = new TMessageResponse().Name;
            if (referenceId != null)
                subject += $"-{referenceId}";

            var observable = connection.Observe(subject.ToLowerInvariant())
                .Select(msg =>
                {
                    var response = new TMessageResponse();
                    response.SetData(msg.Data);
                    return response;
                })
                .Timeout(TimeSpan.FromMilliseconds(timeout == -1 ? int.MaxValue : timeout))
                .Catch<TMessageResponse, TimeoutException>(_ => Observable.Empty<TMessageResponse>());
            using var _ = observable.Subscribe();
            connection.Publish(message, referenceId);

            foreach (var response in observable.ToEnumerable())
            {
                yield return response;

                if (response.IsLastMessage)
                    yield break;
            }
        }

        public static IDisposable Subscribe<TMessage>(this IConnection connection, Action<TMessage> func, Guid? referenceId = null)
            where TMessage : notnull, IMessage, new()
        {
            var subject = new TMessage().Name;
            if (referenceId != null)
                subject += $"-{referenceId}";

            return connection.SubscribeAsync(subject.ToLowerInvariant(), (s, e) =>
            {
                var request = new TMessage();
                request.SetData(e.Message.Data);
                func(request);
            });
        }
        public static IDisposable SubscribeAndReply<TMessageRequest>(this IConnection connection, Func<TMessageRequest, IMessage> func, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
        {
            var subject = new TMessageRequest().Name;
            if (referenceId != null)
                subject += $"-{referenceId}";

            return connection.SubscribeAsync(subject.ToLowerInvariant(), (s, e) =>
            {
                var request = new TMessageRequest();
                request.SetData(e.Message.Data);
                var response = func(request);

                connection.Publish(response, referenceId);
            });
        }
        public static IDisposable SubscribeAndReplyEnumerable<TMessageRequest, TMessageResponse>(this IConnection connection, Func<TMessageRequest, IEnumerable<TMessageResponse>> func, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IEnumerableMessage, new()
        {
            var subject = new TMessageRequest().Name;

            return connection.SubscribeAsync(subject.ToLowerInvariant(), (s, e) =>
            {
                var request = new TMessageRequest();
                request.SetData(e.Message.Data);
                foreach (var response in func(request))
                    connection.Publish(response, referenceId);
            });
        }
        public static IDisposable SubscribeAndReply<TMessageRequest>(this IConnection connection, IMessageHandler<TMessageRequest> handler, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
        {
            var subject = new TMessageRequest().Name;
            if (referenceId != null)
                subject += $"-{referenceId}";

            return connection.SubscribeAsync(subject.ToLowerInvariant(), (s, e) =>
            {
                var request = new TMessageRequest();
                request.SetData(e.Message.Data);
                var response = handler.Handle(request);

                connection.Publish(response, referenceId);
            });
        }
    }

    public static class IConnectionMessageExtensionsAsync
    {
        public static Task PublishAsync(this IConnection conn, IMessage message, Guid? referenceId = null)
        {
            var subject = message.Name;
            if (referenceId != null)
                subject += $"-{referenceId}";

            return Task.Run(() => conn.Publish(subject.ToLowerInvariant(), message.GetData().ToArray()));
        }
        public static async Task<TMessageResponse?> PublishAndWaitForReplyAsync<TMessageRequest, TMessageResponse>(this IConnection connection, TMessageRequest message, Guid? referenceId = null, int timeout = -1)
            where TMessageRequest : notnull, IMessage
            where TMessageResponse : class, IMessage, new()
        {
            var subject = new TMessageResponse().Name;
            if (referenceId != null)
                subject += $"-{referenceId}";

            var cancellationTokenSource = new CancellationTokenSource(timeout);
            var returnLock = new TaskCompletionSource<TMessageResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
            using var sub = connection.SubscribeAsync(subject.ToLowerInvariant(), (s, e) =>
            {
                var response = new TMessageResponse();
                response.SetData(e.Message.Data);
                returnLock.SetResult(response);
            });
            await connection.PublishAsync(message, referenceId);

            return await returnLock.Task.WaitAsync(cancellationTokenSource.Token);
        }
        public static async IAsyncEnumerable<TMessageResponse> PublishAndWaitForReplyEnumerableAsync<TMessageRequest, TMessageResponse>(this IConnection connection, TMessageRequest message, Guid? referenceId = null, int timeout = -1)
            where TMessageRequest : notnull, IMessage
            where TMessageResponse : class, IEnumerableMessage, new()
        {
            var subject = new TMessageResponse().Name;
            if (referenceId != null)
                subject += $"-{referenceId}";

            var observable = connection.Observe(subject.ToLowerInvariant())
                .Select(msg =>
                {
                    var response = new TMessageResponse();
                    response.SetData(msg.Data);
                    return response;
                })
                .Timeout(TimeSpan.FromMilliseconds(timeout == -1 ? int.MaxValue : timeout))
                .Catch<TMessageResponse, TimeoutException>(_ => Observable.Empty<TMessageResponse>());
            using var _ = observable.Subscribe();
            await connection.PublishAsync(message, referenceId);

            await foreach(var response in observable.ToAsyncEnumerable())
            {
                yield return response;

                if (response.IsLastMessage)
                    yield break;
            }
        }

        public static async Task<IDisposable> SubscribeAsync<TMessage>(this IConnection connection, Func<TMessage, Task> func, Guid? referenceId = null)
            where TMessage : notnull, IMessage, new()
        {
            var subject = new TMessage().Name;
            if (referenceId != null)
                subject += $"-{referenceId}";

            return connection.SubscribeAsync(subject.ToLowerInvariant(), async (s, e) =>
            {
                var request = new TMessage();
                request.SetData(e.Message.Data);
                await func(request);
            });
        }
        public static async Task<IDisposable> SubscribeAndReplyAsync<TMessageRequest>(this IConnection connection, Func<TMessageRequest, Task<IMessage>> func, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
        {
            var subject = new TMessageRequest().Name;
            if (referenceId != null)
                subject += $"-{referenceId}";

            return connection.SubscribeAsync(subject.ToLowerInvariant(), async (s, e) =>
            {
                var request = new TMessageRequest();
                request.SetData(e.Message.Data);
                var response = await func(request);
                await connection.PublishAsync(response, referenceId);
            });
        }
        public static async Task<IDisposable> SubscribeAndReplyEnumerableAsync<TMessageRequest, TMessageResponse>(this IConnection connection, Func<TMessageRequest, IAsyncEnumerable<TMessageResponse>> func, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IEnumerableMessage, new()
        {
            var subject = new TMessageRequest().Name;

            return connection.SubscribeAsync(subject.ToLowerInvariant(), async (s, e) =>
            {
                var request = new TMessageRequest();
                request.SetData(e.Message.Data);
                await foreach (var response in func(request))
                    await connection.PublishAsync(response, referenceId);
            });
        }
        public static async Task<IDisposable> SubscribeAndReplyAsync<TMessageRequest>(this IConnection connection, IMessageHandler<TMessageRequest> handler, Guid? referenceId = null)
            where TMessageRequest : class, IMessage, new()
        {
            var subject = new TMessageRequest().Name;
            if (referenceId != null)
                subject += $"-{referenceId}";

            return connection.SubscribeAsync(subject.ToLowerInvariant(), async (s, e) =>
            {
                var request = new TMessageRequest();
                request.SetData(e.Message.Data);
                var response = await handler.HandleAsync(request);

                await connection.PublishAsync(response, referenceId);
            });
        }
    }
}
*/