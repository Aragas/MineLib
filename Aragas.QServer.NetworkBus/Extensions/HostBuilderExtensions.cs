using Aragas.QServer.NetworkBus;
using Aragas.QServer.NetworkBus.Data;

using Microsoft.Extensions.DependencyInjection;

using NATS.Client;

using System;

namespace Microsoft.Extensions.Hosting
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseServiceOptions(this IHostBuilder hostBuilder, Guid uid) =>
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.Configure<ServiceOptions>(o => o.Uid = uid);
            });

        public static IHostBuilder UseNATSNetworkBus(this IHostBuilder hostBuilder) =>
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton(ConnectionFactory.GetDefaultOptions().SetDefaultArgs());
                services.AddSingleton<IAsyncNetworkBus>(sp => new AsyncNATSBus(sp.GetRequiredService<NATS.Client.Options>()));
                services.AddSingleton<INetworkBus>(sp => sp.GetRequiredService<IAsyncNetworkBus>());
                services.AddSingleton<SubscriptionStorage>();
            });
    }
}