using App.Metrics;
using App.Metrics.Histogram;

using Microsoft.Extensions.Hosting;

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Prometheus
{
    public class CpuUsageMetricsService : BackgroundService
    {
        private HistogramOptions process_start_time_milliseconds = new HistogramOptions()
        {
            Name = "Process CPU Usage Percent",
            MeasurementUnit = Unit.Percent
        };

        private readonly IMetrics _metrics;
        private readonly int _delay;
        private readonly Process _process;

        public CpuUsageMetricsService(IMetrics metrics, int delay = 3000)
        {
            _metrics = metrics;
            _delay = delay;

            _process = Process.GetCurrentProcess();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _process.Refresh();

                var startTime = DateTime.UtcNow;
                var startCpuUsage = _process.TotalProcessorTime;

                await Task.Delay(_delay);

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