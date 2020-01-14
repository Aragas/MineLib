using App.Metrics;
using App.Metrics.Gauge;
using App.Metrics.Histogram;

using Microsoft.Extensions.Hosting;

using Npgsql;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Aragas.QServer.Prometheus
{
    public class PostgreSQLMetricsService : BackgroundService
    {
        private readonly IMetrics _metrics;
        private readonly int _delay;
        private readonly List<(string ConnectionString, HistogramOptions Histogram, GaugeOptions Gauge)> _connectionsDictionary = new List<(string, HistogramOptions, GaugeOptions)>();

        public PostgreSQLMetricsService(IMetrics metrics, (string ConnectionName, string ConnectionString)[] connections, int delay = 3000)
        {
            _metrics = metrics;
            _delay = delay;

            foreach (var (ConnectionName, ConnectionString) in connections)
            {
                var histogram = new HistogramOptions()
                {
                    Name = $"Service PostgreSQL {ConnectionName} Response Milliseconds",
                    MeasurementUnit = Unit.Custom("Milliseconds")
                };
                var gauge = new GaugeOptions()
                {
                    Name = $"Service PostgreSQL {ConnectionName} Last Response Milliseconds",
                    MeasurementUnit = Unit.Custom("Milliseconds")
                };
                _metrics.Measure.Gauge.SetValue(gauge, double.PositiveInfinity);
                _connectionsDictionary.Add((ConnectionString, histogram, gauge));
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var stopwatch = new Stopwatch();
            while (!stoppingToken.IsCancellationRequested)
            {
                stopwatch.Restart();
                foreach (var keyValue in _connectionsDictionary)
                {
                    try
                    {
                        using var connection = new NpgsqlConnection(keyValue.ConnectionString);
                        await connection.OpenAsync(stoppingToken);

                        using var command = connection.CreateCommand();
                        command.CommandText = "SELECT 1;";
                        await command.ExecuteScalarAsync(stoppingToken);

                        var response = stopwatch.ElapsedMilliseconds;
                        _metrics.Measure.Histogram.Update(keyValue.Histogram, response);
                        _metrics.Measure.Gauge.SetValue(keyValue.Gauge, response);
                    }
                    catch (Exception)
                    {
                        _metrics.Measure.Gauge.SetValue(keyValue.Gauge, double.PositiveInfinity);
                    }
                }

                await Task.Delay(_delay);
            }
        }
    }
}