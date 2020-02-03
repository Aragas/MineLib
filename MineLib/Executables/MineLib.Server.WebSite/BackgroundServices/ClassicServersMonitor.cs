using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MineLib.Server.Heartbeat.Infrastructure.Data;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MineLib.Server.Heartbeat.BackgroundServices
{
    public class ClassicServersMonitor : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public ClassicServersMonitor(ILogger<ClassicServersMonitor> logger, IServiceProvider serviceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var classicServersRepository = scope.ServiceProvider.GetRequiredService<IClassicServersRepository>();

                    foreach (var classicServer in classicServersRepository.List())
                    {
                        if (DateTimeOffset.UtcNow > classicServer.LastUpdate + TimeSpan.FromMinutes(2))
                        {
                            classicServersRepository.Delete(classicServer);
                            _logger.LogInformation("{Type}: Deleted server '{Name}'.", GetType().FullName, classicServer.Name);
                        }
                    }
                }

                await Task.Delay(45000);
            }
        }
    }
}