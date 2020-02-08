using PokeD.Server.Core.NetworkBus.Messages;

using System;

namespace Aragas.QServer.NetworkBus
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

        public static void HandleGetWorldState<TMessageHandler>(this SubscriptionStorage subscriptionStorage, TMessageHandler? messageHandler = null)
            where TMessageHandler : class, IMessageHandler<GetWorldStateRequestMessage, GetWorldStateResponseMessage>
        {
            subscriptionStorage.Handle<TMessageHandler, GetWorldStateRequestMessage, GetWorldStateResponseMessage>(messageHandler: messageHandler);
        }
    }
}