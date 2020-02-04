using App.Metrics.Health;

using Aragas.QServer.Metrics;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Health
{
    public sealed class RamHealthCheck : HealthCheck
    {
        private readonly MemoryMetricsClient _memoryMetricsClient = new MemoryMetricsClient();
        private readonly int _delay;

        public RamHealthCheck(int delay = 300) : base(nameof(RamHealthCheck))
        {
            _delay = delay;
        }

        protected override ValueTask<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var metrics = _memoryMetricsClient.GetMetrics();
                var percentUsed = 100 * metrics.Used / metrics.Total;

                var message = $"RAM Usage: {percentUsed}%. Total: {metrics.Total * 1024 * 1024} bytes, Used: {metrics.Used * 1024 * 1024} bytes, Free: {metrics.Free * 1024 * 1024} bytes";

                if (percentUsed > 90)
                    return new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(message));
                if (percentUsed > 80)
                    return new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(message));

                return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy(message));
            }
            catch (Exception e)
            {
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(e));
            }
        }
    }
}