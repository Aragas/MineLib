using Aragas.QServer.NetworkBus;
using Aragas.QServer.NetworkBus.Data;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using MineLib.Server.Core;

using System.Threading.Tasks;

namespace MineLib.Server.WorldBus
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await MineLibHostProgram.Main<Program>(CreateHostBuilder, BeforeRun, args);
        }

        public static IHostBuilder CreateHostBuilder(IHostBuilder hostBuilder) => hostBuilder
            // Options
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<ServiceOptions>(o => o.Name = "WorldBus");
            })

            .ConfigureServices((hostContext, services) =>
            {
                services.AddNpgSqlMetrics("Database", hostContext.Configuration["PostgreSQLConnectionString"]);
            })

            .ConfigureServices(services =>
            {
                services.AddSingleton<IWorldHandler, StandardWorldHandler>();

                services.AddSingleton<WorldHandlerManager>();
            })

            .UseConsoleLifetime();

        private static void BeforeRun(IHost host)
        {
            var serviceOptions = host.Services.GetRequiredService<IOptions<ServiceOptions>>().Value;
            var subscriptionStorage = host.Services.GetRequiredService<SubscriptionStorage>();
            subscriptionStorage.HandleChunksInSquare<WorldHandlerManager>();
            subscriptionStorage.HandleChunksInCircle<WorldHandlerManager>();
        }
    }
}