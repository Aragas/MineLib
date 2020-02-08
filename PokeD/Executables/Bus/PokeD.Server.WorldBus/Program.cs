using Aragas.QServer.NetworkBus;
using Aragas.QServer.NetworkBus.Data;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using PokeD.Server.Core;
using PokeD.Server.WorldBus.BackgroundServices;

using System.Threading.Tasks;

namespace PokeD.Server.WorldBus
{
    internal sealed class Program
    {
        public static async Task Main(string[] args)
        {
            await PokeDHostProgram.Main<Program>(CreateHostBuilder, BeforeRun, args);
        }

        public static IHostBuilder CreateHostBuilder(IHostBuilder hostBuilder) => hostBuilder
            // Options
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<ServiceOptions>(o => o.Name = "WorldBus");
            })

            .ConfigureServices(services =>
            {
                services.AddHostedService<WorldService>();
            })

            .UseConsoleLifetime();

        private static void BeforeRun(IHost host)
        {
            var serviceOptions = host.Services.GetRequiredService<IOptions<ServiceOptions>>().Value;
            var subscriptionStorage = host.Services.GetRequiredService<SubscriptionStorage>();
            subscriptionStorage.HandleGetWorldState<WorldService>();
        }
    }
}
