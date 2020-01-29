using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aragas.QServer.Core.Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace MineLib.Server.Heartbeat
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            CreateLogger();

            try
            {
                Log.Information("Service is starting.");
                var hostBuilder = CreateHostBuilder(args);
                var host = hostBuilder.Build();
                BeforeRun(host.Services);
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Fatal unhandled exception occurred.");
            }
            finally
            {
                Log.Information("Service has stopped.");
                Log.CloseAndFlush();
            }
        }

        private static void CreateLogger()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(environmentName == "Production" ? "appsettings.json" : $"appsettings.{environmentName}.json", optional: false)
                .Build();

            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.With<ApplicationInfoEnricher>()
                .Enrich.FromLogContext();

            Log.Logger = loggerConfig.CreateLogger();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.AddEnvironmentVariables(prefix: "AZUREDISCORDBOT_");
            })
            .ConfigureServices((hostingContext, services) =>
            {
                services.AddAzureDiscordBot(hostingContext.Configuration);
                services.BuildServiceProvider();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .ConfigureServices(services =>
                    {
                        services.AddControllers();
                    })
                    .Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    })
                    .UseKestrel();
            })
            .UseSerilog();

        private static void BeforeRun(IServiceProvider serviceProvider)
        {
            var azureWebHookService = serviceProvider.GetService<AzureRestApiService>();
            var lifetime = serviceProvider.GetService<IHostApplicationLifetime>();
            lifetime.ApplicationStarted.Register(async () =>
            {
                await azureWebHookService.DeleteSubscriptionsAsync();
                await azureWebHookService.CreateSubscriptionsAsync();
            });
            lifetime.ApplicationStopped.Register(async () => await azureWebHookService.DeleteSubscriptionsAsync());

            var db = serviceProvider.CreateScope().ServiceProvider.GetService<DatabaseUserProvider>();
            if (db != null)
                db.Database.EnsureCreated();
        }

        /*
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        */
    }
}
