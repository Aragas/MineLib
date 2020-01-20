using Aragas.QServer.Core.Data;
using Aragas.QServer.Core.NetworkBus;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using MineLib.Server.Core;

using System;
using System.Threading.Tasks;

namespace MineLib.Server.EntityBus
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
                services.Configure<ServiceOptions>(o => o.Name = "EntityBus");
            })

            // Metrics
            .ConfigureServices((hostContext, services) =>
            {
                services.AddNpgSqlMetrics("Database", hostContext.Configuration["PostgreSQLConnectionString"]);
            })

            // 
            .ConfigureServices(services =>
            {
                services.AddSingleton<EntityHandlerManager>();
            })

            .UseConsoleLifetime();

        private static void BeforeRun(IServiceProvider serviceProvider)
        {
            var serviceOptions = serviceProvider.GetRequiredService<IOptions<ServiceOptions>>().Value;
            var subscriptionStorage = serviceProvider.GetRequiredService<SubscriptionStorage>();
            subscriptionStorage.HandleGetNewEntityId<EntityHandlerManager>();
        }
    }
}