using Aragas.QServer.NetworkBus.Handlers;
using Aragas.QServer.NetworkBus.Messages;

using System;

namespace Aragas.QServer.NetworkBus
{
    public static class SubscriptionStorageExtensions
    {
        public static void ReceiveServicesPong<TMessageReceiver>(this SubscriptionStorage subscriptionStorage, TMessageReceiver? messageReceiver = null)
            where TMessageReceiver : class, IMessageReceiver<ServicesPongMessage>
        {
            subscriptionStorage.Receive<TMessageReceiver, ServicesPongMessage>(messageReceiver: messageReceiver);
        }

        public static void HandleServiceDiscoveryHandler(this SubscriptionStorage subscriptionStorage) =>
            subscriptionStorage.Handle<ServiceDiscoveryHandler, ServicesPingMessage, ServicesPongMessage>();

        public static void HandleMetricsPrometheusHandler(this SubscriptionStorage subscriptionStorage, Guid referenceId) =>
            subscriptionStorage.Handle<MetricsPrometheusHandler, AppMetricsPrometheusRequestMessage, AppMetricsPrometheusResponseMessage>(referenceId);
        
        public static void HandleHealthHandler(this SubscriptionStorage subscriptionStorage, Guid referenceId) =>
            subscriptionStorage.Handle<HealthHandler, AppMetricsHealthRequestMessage>(referenceId);
    }
}