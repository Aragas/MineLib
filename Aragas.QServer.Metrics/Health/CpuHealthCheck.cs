﻿using App.Metrics.Health;

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Health
{
    public sealed class CpuHealthCheck : HealthCheck, IDisposable
    {
        public static double CpuUsagePercent;

        private readonly Task _cpuUsageHealthTask;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public CpuHealthCheck(int delay = 3000) : base(nameof(CpuHealthCheck))
        {
            _cpuUsageHealthTask = Task.Factory.StartNew(async () =>
            {
                var process = Process.GetCurrentProcess();
                while (!_cancellationTokenSource.IsCancellationRequested)
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

                    CpuUsagePercent = cpuUsageTotal * 100;
                }
            }, TaskCreationOptions.LongRunning);
        }

        protected override ValueTask<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default)
        {
            if (_cpuUsageHealthTask.IsCanceled || _cpuUsageHealthTask.IsFaulted)
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded("CPU Usage checker faulted!"));

            var usage = CpuUsagePercent;
            var message = $"CPU Usage {usage}%";
            if (usage > 90)
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(message));
            if (usage > 80)
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(message));

            return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy(message));
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cpuUsageHealthTask.Dispose();
        }
    }
}