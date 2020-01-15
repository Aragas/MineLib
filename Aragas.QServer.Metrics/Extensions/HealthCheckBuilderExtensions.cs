using Aragas.QServer.Metrics;

using NATS.Client;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace App.Metrics.Health
{
    public static class HealthCheckBuilderExtensions
    {
        private static Task? CpuUsageHealthTask;
        public static double CurrentCpuUsagePercent;

        public static IHealthBuilder AddCpuUsageHealthCheck(this IHealthCheckBuilder healthCheckBuilder,
            string name,
            int delay = 3000,
            bool degradedOnError = false)
        {
            if (CpuUsageHealthTask == null || CpuUsageHealthTask.IsFaulted || CpuUsageHealthTask.IsCanceled)
            {
                CpuUsageHealthTask?.Dispose();
                CpuUsageHealthTask = Task.Factory.StartNew(async () =>
                {
                    var process = Process.GetCurrentProcess();
                    while (true)
                    {
                        var startTime = DateTime.UtcNow;
                        process.Refresh();
                        var startCpuUsage = process.TotalProcessorTime;

                        await Task.Delay(delay);

                        var endTime = DateTime.UtcNow;
                        process.Refresh();
                        var endCpuUsage = process.TotalProcessorTime;

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
            int delay = 3000,
            bool degradedOnError = false)
        {
            var client = new MemoryMetricsClient();

            healthCheckBuilder.AddCheck(
                name,
                () =>
                {
                    try
                    {
                        var metrics = client.GetMetrics();
                        var percentUsed = 100 * metrics.Used / metrics.Total;

                        var message = $"RAM Usage: {percentUsed}%. Total: {metrics.Total * 1024 * 1024} bytes, Used: {metrics.Used * 1024 * 1024} bytes, Free: {metrics.Free * 1024 * 1024} bytes";

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