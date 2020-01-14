using App.Metrics;
using App.Metrics.Histogram;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Prometheus
{
    public class MemoryUsageMetricsService : BackgroundService
    {
        private class MemoryMetricsClient
        {
            public class MemoryMetrics
            {
                public double Total;
                public double Used;
                public double Free;
            }

            private static bool IsUnix() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            public MemoryMetrics GetMetrics() => IsUnix() ? GetUnixMetrics() : GetWindowsMetrics();

            private MemoryMetrics GetWindowsMetrics()
            {
                var output = "";

                var info = new ProcessStartInfo
                {
                    FileName = "wmic",
                    Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value",
                    RedirectStandardOutput = true
                };

                using (var process = Process.Start(info))
                {
                    output = process.StandardOutput.ReadToEnd();
                }

                var lines = output.Trim().Split("\n");
                var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
                var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

                var metrics = new MemoryMetrics
                {
                    Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0),
                    Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0)
                };
                metrics.Used = metrics.Total - metrics.Free;

                return metrics;
            }

            private MemoryMetrics GetUnixMetrics()
            {
                var output = "";

                var info = new ProcessStartInfo("free -m")
                {
                    FileName = "/bin/sh",
                    Arguments = "-c \"free -m\"",
                    RedirectStandardOutput = true
                };

                using (var process = Process.Start(info))
                {
                    output = process.StandardOutput.ReadToEnd();
                    Console.WriteLine(output);
                }

                var lines = output.Split("\n");
                var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                return new MemoryMetrics
                {
                    Total = double.Parse(memory[1]),
                    Used = double.Parse(memory[2]),
                    Free = double.Parse(memory[3])
                };
            }
        }


        private HistogramOptions system_memory_usage_percent = new HistogramOptions()
        {
            Name = "System Memory Usage Percent",
            MeasurementUnit = Unit.Percent
        };

        private readonly IMetrics _metrics;
        private readonly ILogger _logger;
        private readonly int _delay;

        public MemoryUsageMetricsService(IMetrics metrics, ILogger<CpuUsageMetricsService> logger, int delay = 3000)
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

                await Task.Delay(_delay);
            }
        }
    }
}