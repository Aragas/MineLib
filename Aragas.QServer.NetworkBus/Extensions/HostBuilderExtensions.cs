using Aragas.QServer.NetworkBus;
using Aragas.QServer.NetworkBus.Data;
using Aragas.QServer.NetworkBus.Options;

using Microsoft.Extensions.DependencyInjection;

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
                services.Configure<NATSOptions>(hostContext.Configuration.GetSection("NATS"));

                services.AddSingleton<IAsyncNetworkBus, AsyncNATSBus>();
                services.AddSingleton<INetworkBus>(sp => sp.GetRequiredService<IAsyncNetworkBus>());
                services.AddSingleton<SubscriptionStorage>();
            });
    }
}