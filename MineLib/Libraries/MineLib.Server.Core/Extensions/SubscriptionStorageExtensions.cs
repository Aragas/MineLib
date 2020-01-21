using Aragas.QServer.Core.NetworkBus.Messages;

using MineLib.Server.Core.NetworkBus.Messages;

using System;

namespace Aragas.QServer.Core.NetworkBus
{
    public static class SubscriptionStorageExtensions
    {
        public static void HandleGetExistingPlayerHandler<THandler>(this SubscriptionStorage subscriptionStorage)
            where THandler : class, IMessageHandler<GetExistingPlayerHandlerRequestMessage, GetExistingPlayerHandlerResponseMessage>
        {
            subscriptionStorage.Handle<THandler, GetExistingPlayerHandlerRequestMessage, GetExistingPlayerHandlerResponseMessage>();
        }
        public static void HandleGetNewPlayerHandler<TExclusiveHandle>(this SubscriptionStorage subscriptionStorage, Guid referenceId)
            where TExclusiveHandle : class, IExclusiveMessageHandler<GetNewPlayerHandlerRequestMessage, GetNewPlayerHandlerResponseMessage>
        {
            subscriptionStorage.ExclusiveHandle<TExclusiveHandle, GetNewPlayerHandlerRequestMessage, GetNewPlayerHandlerResponseMessage>(referenceId);
        }

        public static void HandleChunksInSquare<TEnumerableMessageHandler>(this SubscriptionStorage subscriptionStorage)
            where TEnumerableMessageHandler : class, IEnumerableMessageHandler<ChunksInSquareRequestMessage, ChunksInSquareResponseMessage>
        {
            subscriptionStorage.EnumerableHandle <TEnumerableMessageHandler, ChunksInSquareRequestMessage, ChunksInSquareResponseMessage>();
        }
        public static void HandleChunksInCircle<TEnumerableMessageHandler>(this SubscriptionStorage subscriptionStorage)
            where TEnumerableMessageHandler : class, IEnumerableMessageHandler<ChunksInCircleRequestMessage, ChunksInCircleResponseMessage>
        {
            subscriptionStorage.EnumerableHandle <TEnumerableMessageHandler, ChunksInCircleRequestMessage, ChunksInCircleResponseMessage>();
        }

        public static void HandleGetNewEntityId<TMessageHandler>(this SubscriptionStorage subscriptionStorage)
            where TMessageHandler : class, IMessageHandler<GetNewEntityIdRequestMessage, GetNewEntityIdResponseMessage>
        {
            subscriptionStorage.Handle<TMessageHandler, GetNewEntityIdRequestMessage, GetNewEntityIdResponseMessage>();
        }
    }
}
