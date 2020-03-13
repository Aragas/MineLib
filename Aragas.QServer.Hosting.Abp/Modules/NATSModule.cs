using Aragas.QServer.NetworkBus;
using Aragas.QServer.NetworkBus.Options;

using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.Modularity;

namespace Aragas.QServer.Hosting.Modules
{
    public class NATSModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var services = context.Services;

            services.Configure<NATSOptions>(configuration.GetSection("NATS"));

            services.AddSingleton<IAsyncNetworkBus, AsyncNATSBus>();
            services.AddSingleton<INetworkBus>(sp => sp.GetRequiredService<IAsyncNetworkBus>());
        }
    }
}