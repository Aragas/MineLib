using App.Metrics.Health;
using App.Metrics.Health.Formatters;
using App.Metrics.Health.Formatters.Ascii;
using App.Metrics.Health.Formatters.Json;

using Aragas.QServer.Core.NetworkBus;
using Aragas.QServer.Core.NetworkBus.Messages;
using NATS.Client;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aragas.QServer.Core.AppMetrics
{
    public class AppMetricsHealthHandler : IMessageHandler<AppMetricsHealthRequestMessage>
    {
        private readonly IHealthRoot _healthRoot;
        private readonly IHealthOutputFormatter _formatter;

        public AppMetricsHealthHandler(IHealthRoot healthRoot)
        {
            _healthRoot = healthRoot;
            _formatter = _healthRoot.DefaultOutputHealthFormatter;
            _formatter = healthRoot.OutputHealthFormatters
                .OfType<HealthStatusTextOutputFormatter>()
                //.OfType<HealthStatusJsonOutputFormatter>()
                .SingleOrDefault();
            if (_formatter == null)
                throw new ArgumentException("Include App.Metrics.Health!", nameof(healthRoot));
        }

        public async Task<IMessage> HandleAsync(AppMetricsHealthRequestMessage message)
        {
            var snapshot = await _healthRoot.HealthCheckRunner.ReadAsync();
            using var stream = new MemoryStream();
            await _formatter.WriteAsync(stream, snapshot);
            return new AppMetricsHealthResponseMessage(Encoding.UTF8.GetString(stream.ToArray()));
        }
    }
}