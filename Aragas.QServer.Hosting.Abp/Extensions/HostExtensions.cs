using Aragas.QServer.NetworkBus.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Serilog;

using System;
using System.Threading;
using System.Threading.Tasks;

using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Aragas.QServer.Hosting.Extensions
{
    public static class HostExtensions
    {
        public static async Task RunQServerAbpAsync(this IHost host, CancellationToken cancellationToken = default)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("loggerconfig.json", false)
                .AddJsonFile($"loggerconfig.{env}.json", true)
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Starting.");

                var application = host.Services.GetService<IAbpApplicationWithExternalServiceProvider>();
                var applicationBuilder = host.Services.GetService<ObjectAccessor<IApplicationBuilder>>();
                if (application != null && application.ServiceProvider == null && applicationBuilder == null)
                {
                    var applicationLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
                    applicationLifetime.ApplicationStopping.Register(() => application.Shutdown());
                    applicationLifetime.ApplicationStopped.Register(() => application.Dispose());

                    application.Initialize(host.Services);
                }

                // Recreate logger with Service Uid now that the ServiceModule is initialized.
                var serviceOptions = host.Services.GetService<IOptions<ServiceOptions>>();
                if (serviceOptions?.Value != null)
                {
                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .Enrich.WithApplicationInfo(serviceOptions.Value.Uid)
                        .CreateLogger();
                }

                await host.RunAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Fatal exception.");
                throw;
            }
            finally
            {
                Log.Information("Stopped.");
                Log.CloseAndFlush();
            }
        }
    }
}