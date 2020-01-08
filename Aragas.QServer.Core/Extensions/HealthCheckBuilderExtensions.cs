using App.Metrics.Health;

using NATS.Client;

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Aragas.QServer.Core.Extensions
{
    public static class HealthCheckBuilderExtensions
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

        private static Task? CpuUsageHealthTask;
        private static double CurrentCpuUsagePercent;

        public static IHealthBuilder AddCpuUsageHealthCheck(this IHealthCheckBuilder healthCheckBuilder,
            string name,
            bool degradedOnError = false)
        {
            if (CpuUsageHealthTask == null || CpuUsageHealthTask.IsFaulted || CpuUsageHealthTask.IsCanceled)
            {
                CpuUsageHealthTask?.Dispose();
                CpuUsageHealthTask = Task.Factory.StartNew(async () =>
                {
                    while (true)
                    {
                        var startTime = DateTime.UtcNow;
                        var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

                        await Task.Delay(1000);

                        var endTime = DateTime.UtcNow;
                        var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

                        var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
                        var totalMsPassed = (endTime - startTime).TotalMilliseconds;

                        var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

                        CurrentCpuUsagePercent = cpuUsageTotal * 100;
                    }
                }, TaskCreationOptions.LongRunning);
            }

            healthCheckBuilder.AddCheck(
                name,
                () =>
                {
                    if (CpuUsageHealthTask != null && (CpuUsageHealthTask.IsCanceled || CpuUsageHealthTask.IsFaulted))
                    {
                        return degradedOnError
                            ? new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded("CPU Usage checker faulted!"))
                            : new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy("CPU Usage checker faulted!"));
                    }

                    var usage = CurrentCpuUsagePercent;
                    var message = $"CPU Usage {usage}%";
                    if (usage > 90)
                        new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(message));
                    if (usage > 80)
                        return new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(message));

                    return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy(message));
                });

            return healthCheckBuilder.Builder;
        }

        public static IHealthBuilder AddSystemMemoryHealthCheck(this IHealthCheckBuilder healthCheckBuilder,
            string name,
            bool degradedOnError = false)
        {
            healthCheckBuilder.AddCheck(
                name,
                () =>
                {
                    try
                    {
                        var client = new MemoryMetricsClient();
                        var metrics = client.GetMetrics();
                        var percentUsed = 100 * metrics.Used / metrics.Total;

                        var message = $"Total: {metrics.Total * 1024 * 1024} bytes, Used: {metrics.Used * 1024 * 1024} bytes, Free: {metrics.Free * 1024 * 1024} bytes";

                        if (percentUsed > 90)
                            new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(message));
                        if (percentUsed > 80)
                            return new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(message));

                        return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy(message));
                    }
                    catch (Exception e) when (e is Exception)
                    {
                        return degradedOnError
                            ? new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(e))
                            : new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(e));
                    }
                });

            return healthCheckBuilder.Builder;
        }

        public static IHealthBuilder AddNatsConnectivityCheck(this IHealthCheckBuilder healthCheckBuilder,
            string name,
            Options connectionOptions,
            int milliseconsTreshold,
            bool degradedOnError = false)
        {
            healthCheckBuilder.AddCheck(
                name,
                () =>
                {
                    try
                    {
                        connectionOptions.AllowReconnect = false;
                        connectionOptions.Timeout = 500;
                        var stopwatch = Stopwatch.StartNew();
                        try
                        {
                            using var connection = new ConnectionFactory().CreateConnection(connectionOptions);
                        }
                        catch (Exception e) when (e is NATSTimeoutException)
                        {
                            return new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy($"FAILED. Timed out ({connectionOptions.Timeout} ms)."));
                        }
                        finally
                        {
                            stopwatch.Stop();
                        }

                        var percentUsed = 100 * stopwatch.ElapsedMilliseconds / milliseconsTreshold;

                        if (percentUsed >= 100)
                            new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy($"FAILED. {stopwatch.ElapsedMilliseconds} > {milliseconsTreshold} ms"));
                        if (percentUsed > 80)
                            return new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded($"WARNING. Connection successful in {stopwatch.ElapsedMilliseconds} ms"));

                        return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy($"OK. Connection successful in {stopwatch.ElapsedMilliseconds} ms"));
                    }
                    catch (Exception e) when (e is Exception)
                    {
                        return degradedOnError
                            ? new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(e))
                            : new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(e));
                    }
                });

            return healthCheckBuilder.Builder;
        }
    }
}