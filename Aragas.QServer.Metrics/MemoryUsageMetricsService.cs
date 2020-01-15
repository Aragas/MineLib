using App.Metrics;
using App.Metrics.Histogram;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Metrics
{
    public class MemoryUsageMetricsService : BackgroundService
    {
        private HistogramOptions system_memory_usage_percent = new HistogramOptions()
        {
            Name = "System Memory Usage Percent",
            MeasurementUnit = Unit.Percent
        };

        private readonly IMetrics _metrics;
        private readonly ILogger _logger;
        private readonly int _delay;

        public MemoryUsageMetricsService(IMetrics metrics, ILogger<MemoryUsageMetricsService> logger, int delay = 3000)
        {
            _metrics = metrics;
            _logger = logger;
            _delay = delay;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{TypeName}: Starting reporting. Delay:{Delay}", GetType().Name, _delay);

            var client = new MemoryMetricsClient();
            while (!stoppingToken.IsCancellationRequested)
            {
                var metrics = client.GetMetrics();
                _metrics.Measure.Histogram.Update(system_memory_usage_percent, (long) (metrics.Used / metrics.Total * 100D * 100D));

                await Task.Delay(_delay, stoppingToken);
            }
        }
    }
}