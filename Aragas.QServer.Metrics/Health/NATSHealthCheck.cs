/*
using App.Metrics.Health;

using NATS.Client;

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Health
{
    public class NATSHealthCheck : HealthCheck
    {
        private readonly Options _options;
        private readonly int _milliseconsTreshold;

        public NATSHealthCheck(Options options, int milliseconsTreshold = 300) : base(nameof(NATSHealthCheck))
        {
            _options = options;
            _milliseconsTreshold = milliseconsTreshold;
        }

        protected override ValueTask<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    using var connection = new ConnectionFactory().CreateConnection(_options);
                }
                catch (Exception e) when (e is NATSTimeoutException)
                {
                    return new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy($"FAILED. Timed out ({_options.Timeout} ms)."));
                }
                finally
                {
                    stopwatch.Stop();
                }

                var percentUsed = 100 * stopwatch.ElapsedMilliseconds / _milliseconsTreshold;

                if (percentUsed >= 100)
                    new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy($"FAILED. {stopwatch.ElapsedMilliseconds} > {_milliseconsTreshold} ms"));
                if (percentUsed > 80)
                    return new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded($"WARNING. Connection successful in {stopwatch.ElapsedMilliseconds} ms"));

                return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy($"OK. Connection successful in {stopwatch.ElapsedMilliseconds} ms"));
            }
            catch (Exception e) when (e is Exception)
            {
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(e));
            }
        }
    }
}
*/