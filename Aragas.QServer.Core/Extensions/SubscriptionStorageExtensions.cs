using Aragas.QServer.Core.NetworkBus.Handlers;
using Aragas.QServer.Core.NetworkBus.Messages;

using System;

namespace Aragas.QServer.Core.NetworkBus
{
    public static class SubscriptionStorageExtensions
    {
        public static void HandleServiceDiscoveryHandler(this SubscriptionStorage subscriptionStorage) =>
            subscriptionStorage.Handle<ServiceDiscoveryHandler, ServicesPingMessage, ServicesPongMessage>();
       
        public static void HandleMetricsPrometheusHandler(this SubscriptionStorage subscriptionStorage, Guid referenceId) =>
            subscriptionStorage.Handle<MetricsPrometheusHandler, AppMetricsPrometheusRequestMessage, AppMetricsPrometheusResponseMessage>(referenceId);
        
        public static void HandleHealthHandler(this SubscriptionStorage subscriptionStorage, Guid referenceId) =>
            subscriptionStorage.Handle<HealthHandler, AppMetricsHealthRequestMessage>(referenceId);
    }
}
