using App.Metrics;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Prometheus;

using Aragas.QServer.Core.NetworkBus.Messages;

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aragas.QServer.Core.NetworkBus.Handlers
{
    public class MetricsPrometheusHandler : IMessageHandler<AppMetricsPrometheusRequestMessage, AppMetricsPrometheusResponseMessage>
    {
        private readonly IMetricsRoot _metricsRoot;
        private readonly IMetricsOutputFormatter _formatter;

        public MetricsPrometheusHandler(IMetricsRoot metricsRoot)
        {
            _metricsRoot = metricsRoot;
            _formatter = _metricsRoot.OutputMetricsFormatters
                .OfType<MetricsPrometheusTextOutputFormatter>()
                .SingleOrDefault();
            if (_formatter == null)
                throw new ArgumentException("Include App.Metrics.Formatters.Prometheus!", nameof(metricsRoot));
        }

        public async Task<AppMetricsPrometheusResponseMessage> HandleAsync(AppMetricsPrometheusRequestMessage message)
        {
            var snapshot = _metricsRoot.Snapshot.Get();
            using var stream = new MemoryStream();
            await _formatter.WriteAsync(stream, snapshot);
            return new AppMetricsPrometheusResponseMessage(Encoding.UTF8.GetString(stream.ToArray()));
        }
    }
}