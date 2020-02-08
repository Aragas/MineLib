using Aragas.QServer.NetworkBus.Data;

using I18Next.Net.Backends;
using I18Next.Net.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using PokeD.Server.Core;
using PokeD.Server.Proxy.Data;

using System;
using System.Net;
using System.Threading.Tasks;

namespace PokeD.Server.Proxy
{
    internal sealed class Program
    {
        public static async Task Main(string[] args)
        {
            ServicePointManager.UseNagleAlgorithm = false;
            //PokeD.Server.Proxy.Extensions.PacketExtensions.Init();
            await PokeDHostProgram.Main<Program>(CreateHostBuilder, BeforeRun, args);
        }

        public static IHostBuilder CreateHostBuilder(IHostBuilder hostBuilder) => hostBuilder
            // Options
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<ServiceOptions>(o => o.Name = "Proxy");
                services.Configure<PokeDOptions>(hostContext.Configuration.GetSection("MineLib"));

                services.AddSingleton<ServerInfo>();
            })

            // Localization
            .ConfigureServices(services =>
            {
                services.AddI18NextLocalization(i18N => i18N.AddBackend(new JsonFileBackend("locales")));
            })

            // Listeners
            .ConfigureServices(services =>
            {
                services.AddHostedService<ProxyP3DListenerService>();
            })

            .UseConsoleLifetime();

        private static void BeforeRun(IHost host)
        {
            var serviceOptions = host.Services.GetRequiredService<IOptions<ServiceOptions>>().Value;
        }
    }
}