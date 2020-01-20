using Aragas.QServer.Core.Data;
using Aragas.QServer.Core.NetworkBus;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using MineLib.Server.Core;

using System;
using System.Threading.Tasks;

namespace MineLib.Server.WorldBus
{
    public class Program : MineLibHostProgram
    {
        public static async Task Main(string[] args)
        {
            await Main<Program>(CreateHostBuilder, BeforeRun, args);
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

        private static void BeforeRun(IServiceProvider serviceProvider)
        {
            var serviceOptions = serviceProvider.GetRequiredService<IOptions<ServiceOptions>>().Value;
            var subscriptionStorage = serviceProvider.GetRequiredService<SubscriptionStorage>();
            subscriptionStorage.HandleChunksInSquare<WorldHandlerManager>();
            subscriptionStorage.HandleChunksInCircle<WorldHandlerManager>();
        }
    }
}