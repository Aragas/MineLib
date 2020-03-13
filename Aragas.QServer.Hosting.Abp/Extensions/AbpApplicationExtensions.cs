using Aragas.QServer.NetworkBus.Data;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Serilog;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp
{
    public static class AbpApplicationExtensions
    {
        /// <summary>
        /// Returns a Task that completes when shutdown is triggered via the given token.
        /// </summary>
        /// <param name="application">The running <see cref="IAbpApplication"/>.</param>
        /// <param name="token">The token to trigger shutdown.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task WaitForShutdownAsync(this IAbpApplication application, CancellationToken token = default)
        {
            var applicationLifetime = application.ServiceProvider.GetService<IHostApplicationLifetime>();

            token.Register(state => ((IHostApplicationLifetime) state!).StopApplication(), applicationLifetime);

            var waitForStop = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);
            applicationLifetime.ApplicationStopping.Register(obj => ((TaskCompletionSource<object?>) obj!).TrySetResult(null), waitForStop);

            await waitForStop.Task;

            // Host will use its default ShutdownTimeout if none is specified.
            application.Shutdown();
        }

        public static async Task RunQServerAsync(this IAbpApplicationWithInternalServiceProvider application, CancellationToken cancellationToken = default)
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

                application.Initialize();

                // Recreate logger with Service Uid now that the ServiceModule is initialized.
                var serviceOptions = application.ServiceProvider.GetService<IOptions<ServiceOptions>>();
                if (serviceOptions?.Value != null)
                {
                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .Enrich.WithApplicationInfo(serviceOptions.Value.Uid)
                        .CreateLogger();
                }

                // Start all IHostedService classes
                var hostedServices = application.ServiceProvider.GetServices<IHostedService>();
                foreach (var hostedService in hostedServices)
                    await hostedService.StartAsync(cancellationToken);

                await application.WaitForShutdownAsync(cancellationToken);
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