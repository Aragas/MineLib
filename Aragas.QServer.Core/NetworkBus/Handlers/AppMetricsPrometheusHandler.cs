using App.Metrics;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Prometheus;
using App.Metrics.Gauge;
using App.Metrics.Health;
using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.NetworkBus.Messages;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aragas.QServer.Core.NetworkBus.Handlers
{
    public class AppMetricsPrometheusHandler : IMessageHandler<AppMetricsPrometheusRequestMessage>
    {
        protected static GaugeOptions NATSPingGauge = new GaugeOptions
        {
            Name = "NATS Ping",
            MeasurementUnit = Unit.Custom("Milliseconds"),
        };
        protected static GaugeOptions PostgreSQLPingGauge = new GaugeOptions
        {
            Name = "PostgreSQL Ping",
            MeasurementUnit = Unit.Custom("Milliseconds"),
        };

        protected static GaugeOptions ProcessCpuUsageGauge = new GaugeOptions
        {
            Name = "Process Cpu Usage",
            MeasurementUnit = Unit.Percent,
        };
        protected static GaugeOptions ProcessWorkingSetSizeGauge = new GaugeOptions
        {
            Name = "Process Working Set",
            MeasurementUnit = Unit.Bytes,
        };
        protected static GaugeOptions ProcessPrivateMemorySizeGauge = new GaugeOptions
        {
            Name = "Process Private Memory Size",
            MeasurementUnit = Unit.Bytes,
            Tags = new MetricTags("reporter", "influxdb")
        };

        private readonly IMetricsRoot _metricsRoot;
        private readonly IMetricsOutputFormatter _formatter;
        private readonly Process _process;

        public AppMetricsPrometheusHandler(IMetricsRoot metricsRoot)
        {
            _metricsRoot = metricsRoot;
            _formatter = _metricsRoot.OutputMetricsFormatters
                .OfType<MetricsPrometheusTextOutputFormatter>()
                //.OfType<MetricsPrometheusProtobufOutputFormatter>()
                .SingleOrDefault();
            if (_formatter == null)
                throw new ArgumentException("Include App.Metrics.Formatters.Prometheus!", nameof(metricsRoot));

            _process = Process.GetCurrentProcess();
        }

        public async Task<IMessage> HandleAsync(AppMetricsPrometheusRequestMessage message)
        {
            //_metricsRoot.Measure.Gauge.SetValue(NATSPingGauge, () => GetNATSPing(ConnectionFactory.GetDefaultOptions().SetDefaultArgs()));
            //_metricsRoot.Measure.Gauge.SetValue(PostgreSQLPingGauge, () => ExecuteSqlCheck(() => new NpgsqlConnection("Host=postgres;Port=5432;Database=minelib;Username=minelib;Password=minelib")));

            _process.Refresh();
            _metricsRoot.Measure.Gauge.SetValue(ProcessWorkingSetSizeGauge, _process.WorkingSet64);
            _metricsRoot.Measure.Gauge.SetValue(ProcessPrivateMemorySizeGauge, _process.PrivateMemorySize64);
            _metricsRoot.Measure.Gauge.SetValue(ProcessCpuUsageGauge, HealthCheckBuilderExtensions.CurrentCpuUsagePercent);

            var snapshot = _metricsRoot.Snapshot.Get();
            using var stream = new MemoryStream();
            await _formatter.WriteAsync(stream, snapshot);
            return new AppMetricsPrometheusResponseMessage(Encoding.UTF8.GetString(stream.ToArray()));
        }
    }
}