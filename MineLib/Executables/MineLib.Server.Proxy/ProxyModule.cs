using Aragas.QServer.Hosting;
using Aragas.QServer.NetworkBus.Data;

using I18Next.Net.Backends;
using I18Next.Net.Extensions;

using Microsoft.Extensions.DependencyInjection;

using MineLib.Server.Proxy.BackgroundServices;
using MineLib.Server.Proxy.Data;

using Volo.Abp.Modularity;

namespace MineLib.Server.Proxy
{
    [DependsOn(typeof(QServerModule))]
    public class ProxyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var services = context.Services;

            services.Configure<ServiceOptions>(o => o.Name = "Proxy");
            services.Configure<MineLibOptions>(configuration.GetSection("MineLib"));

            services.AddSingleton<ServerInfo>();
            services.AddSingleton<ClassicServerInfo>();

            services.AddI18NextLocalization(i18N => i18N.AddBackend(new JsonFileBackend("locales")));

            services.AddHostedService<ProxyNettyListenerService>();
            services.AddHostedService<ProxyClassicListenerService>();
        }
    }
}