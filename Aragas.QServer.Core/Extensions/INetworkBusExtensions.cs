using Aragas.QServer.Core.NetworkBus;
using Aragas.QServer.Core.NetworkBus.Messages;
using Nito.AsyncEx;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Core.Extensions
{
    public static class INetworkBusExtensions
    {
        public static IDisposable SubscribeAndReply<TMessageRequest>(this INetworkBus bus, Func<TMessageRequest, Task<IMessage>> func, Guid? referenceId)
            where TMessageRequest : notnull, IMessage, new()
        {
            return bus.SubscribeAndReply<TMessageRequest>(message => func(message).GetAwaiter().GetResult(), referenceId);
        }
        public static IDisposable SubscribeAndReply<TMessageRequest, TMessageResponse>(this INetworkBus bus, Func<TMessageRequest, Task<TMessageResponse>> func, Guid? referenceId)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IMessage, new()
        {
            return bus.SubscribeAndReply<TMessageRequest>(message => func(message).GetAwaiter().GetResult(), referenceId);
        }
        public static IDisposable RegisterReceiver<TMessageRequest>(this INetworkBus bus, IMessageReceiver<TMessageRequest> handler, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
        {
            return bus.Subscribe<TMessageRequest>(message => handler.HandleAsync(message), referenceId);
        }
        public static IDisposable RegisterHandler<TMessageRequest>(this INetworkBus bus, IMessageHandler<TMessageRequest> handler, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
        {
            return bus.SubscribeAndReply<TMessageRequest>(message => handler.HandleAsync(message), referenceId);
        }
        public static IDisposable RegisterHandler<TMessageRequest, TMessageResponse>(this INetworkBus bus, IMessageHandler<TMessageRequest, TMessageResponse> handler, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IMessage, new()
        {
            return bus.SubscribeAndReply<TMessageRequest, TMessageResponse>(message => handler.HandleAsync(message), referenceId);
        }
        public static IDisposable SubscribeAndReplyEnumerable<TMessageRequest, TMessageResponse>(this INetworkBus bus, Func<TMessageRequest, IAsyncEnumerable<TMessageResponse>> func, Guid? referenceId)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IEnumerableMessage, new()
        {
            return bus.SubscribeAndReplyEnumerable<TMessageRequest, TMessageResponse>(message => func(message).ToEnumerable(), referenceId);
        }

        public static TMessageResponse? PublishAndWaitForExclusiveResponse<TMessageRequest, TMessageResponse>(this INetworkBus bus, TMessageRequest message, int timeout = -1)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : class, IMessage, new()
        {
            using var firstResponseCancellationTokenSource = new CancellationTokenSource(timeout);
            var firstResponseLock = new TaskCompletionSource<ExclusiveResponseMessage<TMessageRequest>>(TaskCreationOptions.RunContinuationsAsynchronously);
            using var _ = bus.Subscribe<ExclusiveResponseMessage<TMessageRequest>>(message =>
            {
                firstResponseLock.SetResult(message);
            }, null);
            bus.Publish(new ExclusiveRequestMessage<TMessageRequest>(), null);

            var firstResponse = firstResponseLock.Task.WaitAsync(firstResponseCancellationTokenSource.Token).GetAwaiter().GetResult();
            if (firstResponse == null) return null;


            using var responseCancellationTokenSource = new CancellationTokenSource(timeout);
            var responseLock = new TaskCompletionSource<ExclusiveAcceptedResponseMessage<TMessageResponse>>(TaskCreationOptions.RunContinuationsAsynchronously);
            using var __ = bus.Subscribe<ExclusiveAcceptedResponseMessage<TMessageResponse>>(message =>
            {
                responseLock.SetResult(message);
            }, firstResponse.ReferenceId);
            bus.Publish(new ExclusiveAcceptedRequestMessage<TMessageRequest>(message), firstResponse.ReferenceId);

            var response = responseLock.Task.WaitAsync(responseCancellationTokenSource.Token).GetAwaiter().GetResult();
            return response.Response;
        }
        public static IDisposable SubscribeAndReplyToExclusive<TMessageRequest, TMessageResponse>(this INetworkBus bus, Func<TMessageRequest, bool> canReply, Func<TMessageRequest, TMessageResponse> func, Guid requestReferenceId)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IMessage, new()
        {
            var disposable1 = bus.Subscribe<ExclusiveRequestMessage<TMessageRequest>>(message =>
            {
                if (canReply(message.Request))
                    bus.Publish(new ExclusiveResponseMessage<TMessageRequest>(requestReferenceId), null);
            }, null);

            var disposable2 = bus.SubscribeAndReply<ExclusiveAcceptedRequestMessage<TMessageRequest>>(message =>
            {
                return new ExclusiveAcceptedResponseMessage<TMessageResponse>(func(message.Request));
            }, requestReferenceId);

            return new CompositeDisposable(disposable1, disposable2);
        }
    }
}