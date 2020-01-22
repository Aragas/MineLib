using App.Metrics;
using App.Metrics.Histogram;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Metrics
{
    public class CpuUsageMetricsService : BackgroundService
    {
        private HistogramOptions process_start_time_milliseconds = new HistogramOptions()
        {
            Name = "Process CPU Usage Percent",
            MeasurementUnit = Unit.Percent
        };

        private readonly IMetrics _metrics;
        private readonly ILogger _logger;
        private readonly int _delay;
        private readonly Process _process;

        public CpuUsageMetricsService(IMetrics metrics, ILogger<CpuUsageMetricsService> logger, int delay = 3000)
        {
            _metrics = metrics;
            _logger = logger;
            _delay = delay;

            _process = Process.GetCurrentProcess();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{TypeName}: Starting reporting. Delay:{Delay}", GetType().Name, _delay);

            while (!stoppingToken.IsCancellationRequested)
            {
                _process.Refresh();

                var startTime = DateTime.UtcNow;
                var startCpuUsage = _process.TotalProcessorTime;

                await Task.Delay(_delay, stoppingToken);

                var endTime = DateTime.UtcNow;
                var endCpuUsage = _process.TotalProcessorTime;

                var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
                var totalMsPassed = (endTime - startTime).TotalMilliseconds;

                var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

                _metrics.Measure.Histogram.Update(process_start_time_milliseconds, (long) (cpuUsageTotal * 100D * 100D));
            }
        }
    }
}