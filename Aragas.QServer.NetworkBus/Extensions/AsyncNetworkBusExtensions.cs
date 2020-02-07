using Aragas.QServer.NetworkBus.Messages;

using Nito.AsyncEx;

using System;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.NetworkBus
{
    public static class AsyncNetworkBusExtensions
    {
        public static Task<IDisposable> RegisterReceiverAsync<TMessageRequest>(this IAsyncNetworkBus bus, IMessageReceiver<TMessageRequest> handler, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
        {
            return bus.SubscribeAsync<TMessageRequest>(handler.HandleAsync, referenceId);
        }
        public static Task<IDisposable> RegisterHandlerAsync<TMessageRequest>(this IAsyncNetworkBus bus, IMessageHandler<TMessageRequest> handler, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
        {
            return bus.SubscribeAndReplyAsync<TMessageRequest>(handler.HandleAsync, referenceId);
        }
        public static Task<IDisposable> RegisterHandlerAsync<TMessageRequest, TMessageResponse>(this IAsyncNetworkBus bus, IMessageHandler<TMessageRequest, TMessageResponse> handler, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IMessage, new()
        {
            return bus.SubscribeAndReplyAsync<TMessageRequest, TMessageResponse>(handler.HandleAsync, referenceId);
        }

        public static Task<IDisposable> SubscribeAndReplyAsync<TMessageRequest, TMessageResponse>(this IAsyncNetworkBus bus, Func<TMessageRequest, Task<TMessageResponse>> func, Guid? referenceId)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IMessage, new()
        {
            return bus.SubscribeAndReplyAsync<TMessageRequest>(async message => await func(message), referenceId);
        }

        public static async Task<TMessageResponse?> PublishAndWaitForExclusiveResponseAsync<TMessageRequest, TMessageResponse>(this IAsyncNetworkBus bus, TMessageRequest message, int timeout = -1)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : class, IMessage, new()
        {
            using var firstResponseCancellationTokenSource = new CancellationTokenSource(timeout);
            var firstResponseLock = new TaskCompletionSource<ExclusiveResponseMessage<TMessageRequest>>(TaskCreationOptions.RunContinuationsAsynchronously);
            using var _ = await bus.SubscribeAsync<ExclusiveResponseMessage<TMessageRequest>>(msg =>
            {
                firstResponseLock.SetResult(msg);
                return Task.CompletedTask;
            }, null);
            await bus.PublishAsync(new ExclusiveRequestMessage<TMessageRequest>(), null);

            var firstResponse = await firstResponseLock.Task.WaitAsync(firstResponseCancellationTokenSource.Token);
            if (firstResponse == null) return null;


            using var responseCancellationTokenSource = new CancellationTokenSource(timeout);
            var responseLock = new TaskCompletionSource<ExclusiveAcceptedResponseMessage<TMessageResponse>>(TaskCreationOptions.RunContinuationsAsynchronously);
            using var __ = await bus.SubscribeAsync<ExclusiveAcceptedResponseMessage<TMessageResponse>>(msg =>
            {
                responseLock.SetResult(msg);
                return Task.CompletedTask;
            }, firstResponse.ReferenceId);
            await bus.PublishAsync(new ExclusiveAcceptedRequestMessage<TMessageRequest>(message), firstResponse.ReferenceId);

            var response = await responseLock.Task.WaitAsync(responseCancellationTokenSource.Token);
            return response.Response;
        }
        public static async Task<IDisposable> SubscribeAndReplyToExclusive<TMessageRequest, TMessageResponse>(this IAsyncNetworkBus bus, Func<TMessageRequest, Task<bool>> canReply, Func<TMessageRequest, TMessageResponse> func, Guid requestReferenceId)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IMessage, new()
        {
            var disposable1 = await bus.SubscribeAsync<ExclusiveRequestMessage<TMessageRequest>>(async msg =>
            {
                if (await canReply(msg.Request))
                    await bus.PublishAsync(new ExclusiveResponseMessage<TMessageRequest>(requestReferenceId), null);
            }, null);

            var disposable2 = await bus.SubscribeAndReplyAsync<ExclusiveAcceptedRequestMessage<TMessageRequest>>(msg =>
            {
                return Task.FromResult((IMessage) new ExclusiveAcceptedResponseMessage<TMessageResponse>(func(msg.Request)));
            }, requestReferenceId);

            return new CompositeDisposable(disposable1, disposable2);
        }
    }
}