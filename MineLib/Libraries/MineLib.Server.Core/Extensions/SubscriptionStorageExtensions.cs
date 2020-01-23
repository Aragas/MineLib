using MineLib.Server.Core.NetworkBus.Messages;

using System;

namespace Aragas.QServer.Core.NetworkBus
{
    public static class SubscriptionStorageExtensions
    {
        public static void HandleGetExistingPlayerHandler<THandler>(this SubscriptionStorage subscriptionStorage, THandler? messageHandler = null)
            where THandler : class, IMessageHandler<GetExistingPlayerHandlerRequestMessage, GetExistingPlayerHandlerResponseMessage>?
        {
            subscriptionStorage.Handle<THandler, GetExistingPlayerHandlerRequestMessage, GetExistingPlayerHandlerResponseMessage>(messageHandler: messageHandler);
        }
        public static void HandleGetNewPlayerHandler<TExclusiveHandler>(this SubscriptionStorage subscriptionStorage, Guid referenceId, TExclusiveHandler? messageHandler = null)
            where TExclusiveHandler : class, IExclusiveMessageHandler<GetNewPlayerHandlerRequestMessage, GetNewPlayerHandlerResponseMessage>
        {
            subscriptionStorage.ExclusiveHandle<TExclusiveHandler, GetNewPlayerHandlerRequestMessage, GetNewPlayerHandlerResponseMessage>(referenceId, messageHandler);
        }

        public static void HandleChunksInSquare<TEnumerableMessageHandler>(this SubscriptionStorage subscriptionStorage, TEnumerableMessageHandler? messageHandler = null)
            where TEnumerableMessageHandler : class, IEnumerableMessageHandler<ChunksInSquareRequestMessage, ChunksInSquareResponseMessage>
        {
            subscriptionStorage.EnumerableHandle <TEnumerableMessageHandler, ChunksInSquareRequestMessage, ChunksInSquareResponseMessage>(messageHandler: messageHandler);
        }
        public static void HandleChunksInCircle<TEnumerableMessageHandler>(this SubscriptionStorage subscriptionStorage, TEnumerableMessageHandler? messageHandler = null)
            where TEnumerableMessageHandler : class, IEnumerableMessageHandler<ChunksInCircleRequestMessage, ChunksInCircleResponseMessage>
        {
            subscriptionStorage.EnumerableHandle <TEnumerableMessageHandler, ChunksInCircleRequestMessage, ChunksInCircleResponseMessage>(messageHandler: messageHandler);
        }

        public static void HandleGetNewEntityId<TMessageHandler>(this SubscriptionStorage subscriptionStorage, TMessageHandler? messageHandler = null)
            where TMessageHandler : class, IMessageHandler<GetNewEntityIdRequestMessage, GetNewEntityIdResponseMessage>
        {
            subscriptionStorage.Handle<TMessageHandler, GetNewEntityIdRequestMessage, GetNewEntityIdResponseMessage>(messageHandler: messageHandler);
        }

        public static void HandlePlayerPosition<TMessageHandler>(this SubscriptionStorage subscriptionStorage, TMessageHandler? messageHandler = null)
            where TMessageHandler : class, IMessageHandler<PlayerPositionRequestMessage, PlayerPositionResponseMessage>
        {
            subscriptionStorage.Handle<TMessageHandler, PlayerPositionRequestMessage, PlayerPositionResponseMessage>(messageHandler: messageHandler);
        }
        public static void HandlePlayerLook<TMessageHandler>(this SubscriptionStorage subscriptionStorage, TMessageHandler? messageHandler = null)
            where TMessageHandler : class, IMessageHandler<PlayerLookRequestMessage, PlayerLookResponseMessage>
        {
            subscriptionStorage.Handle<TMessageHandler, PlayerLookRequestMessage, PlayerLookResponseMessage>(messageHandler: messageHandler);
        }
    }
}
