using App.Metrics;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Prometheus;
using App.Metrics.Gauge;
using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.NetworkBus;
using Aragas.QServer.Core.NetworkBus.Messages;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aragas.QServer.Core.AppMetrics
{
    public class AppMetricsPrometheusHandler : IMessageHandler<AppMetricsPrometheusRequestMessage>
    {
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

        public AppMetricsPrometheusHandler(IMetricsRoot metricsRoot)
        {
            _metricsRoot = metricsRoot;
            _formatter = _metricsRoot.OutputMetricsFormatters
                .OfType<MetricsPrometheusTextOutputFormatter>()
                //.OfType<MetricsPrometheusProtobufOutputFormatter>()
                .SingleOrDefault();
            if (_formatter == null)
                throw new ArgumentException("Include App.Metrics.Formatters.Prometheus!", nameof(metricsRoot));
        }

        public async Task<IMessage> HandleAsync(AppMetricsPrometheusRequestMessage message)
        {
            var process = Process.GetCurrentProcess();
            _metricsRoot.Measure.Gauge.SetValue(ProcessWorkingSetSizeGauge, () => process.WorkingSet64);
            _metricsRoot.Measure.Gauge.SetValue(ProcessPrivateMemorySizeGauge, () => process.PrivateMemorySize64);
            _metricsRoot.Measure.Gauge.SetValue(ProcessCpuUsageGauge, () => HealthCheckBuilderExtensions.CurrentCpuUsagePercent);

            var snapshot = _metricsRoot.Snapshot.Get();
            using var stream = new MemoryStream();
            await _formatter.WriteAsync(stream, snapshot);
            return new AppMetricsPrometheusResponseMessage(Encoding.UTF8.GetString(stream.ToArray()));
        }
    }
}