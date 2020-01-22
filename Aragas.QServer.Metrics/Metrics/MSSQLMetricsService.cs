using App.Metrics;
using App.Metrics.Gauge;
using App.Metrics.Histogram;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Metrics
{
    public class MSSQLMetricsService : BackgroundService
    {
        private readonly IMetrics _metrics;
        private readonly ILogger _logger;
        private readonly int _delay;
        private readonly string _connectionString;
        private readonly HistogramOptions _histogram;
        private readonly GaugeOptions _gauge;

        public MSSQLMetricsService(IMetrics metrics, string connectionName, string connectionString, ILogger<MSSQLMetricsService> logger, int delay = 3000)
        {
            _metrics = metrics;
            _connectionString = connectionString;
            _logger = logger;
            _delay = delay;

            _histogram = new HistogramOptions()
            {
                Name = $"Service MSSQL {connectionName} Response Milliseconds",
                MeasurementUnit = Unit.Custom("Milliseconds")
            };
            _gauge = new GaugeOptions()
            {
                Name = $"Service MSSQL {connectionName} Last Response Milliseconds",
                MeasurementUnit = Unit.Custom("Milliseconds")
            };
            _metrics.Measure.Gauge.SetValue(_gauge, double.PositiveInfinity);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{TypeName}: Starting reporting. Delay:{Delay}", GetType().Name, _delay);

            var stopwatch = new Stopwatch();
            while (!stoppingToken.IsCancellationRequested)
            {
                stopwatch.Restart();
                try
                {
                    using var connection = new SqlConnection(_connectionString);
                    await connection.OpenAsync(stoppingToken);

                    using var command = connection.CreateCommand();
                    command.CommandText = "SELECT 1;";
                    await command.ExecuteScalarAsync(stoppingToken);

                    var response = stopwatch.ElapsedMilliseconds;
                    _metrics.Measure.Histogram.Update(_histogram, response);
                    _metrics.Measure.Gauge.SetValue(_gauge, response);
                }
                catch (Exception)
                {
                    _metrics.Measure.Gauge.SetValue(_gauge, double.PositiveInfinity);
                }

                await Task.Delay(_delay);
            }
        }
    }
}