using App.Metrics;
using App.Metrics.Formatters.Prometheus;
using App.Metrics.Health;
using App.Metrics.Health.Formatters.Ascii;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MineLib.Server.WebSite.BackgroundServices
{
    public class MetricsHttpListenerMonitor : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly HttpListener _httpListener;
        private readonly IMetricsRoot _metricsRoot;
        private readonly IHealthRoot _healthRoot;

        public MetricsHttpListenerMonitor(IMetricsRoot metricsRoot, IHealthRoot healthRoot, ILogger<MetricsHttpListenerMonitor> logger)
        {
            _metricsRoot = metricsRoot ?? throw new ArgumentNullException(nameof(metricsRoot));
            _healthRoot = healthRoot ?? throw new ArgumentNullException(nameof(healthRoot));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpListener = new HttpListener();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _httpListener.Prefixes.Add("http://*:9600/");
            _httpListener.Start();

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _httpListener.Stop();

            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var context = await _httpListener.GetContextAsync();
                var request = context.Request;
                var response = context.Response;
                var output = response.OutputStream;

                try
                {
                    if (false) { }
                    else if (string.Equals(request.RawUrl, "/metrics", StringComparison.OrdinalIgnoreCase) && string.Equals(request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                    {
                        var snapshot = _metricsRoot.Snapshot.Get();
                        await _metricsRoot.OutputMetricsFormatters
                            .OfType<MetricsPrometheusTextOutputFormatter>()
                            .First()
                            .WriteAsync(output, snapshot);
                        response.StatusCode = (int) HttpStatusCode.OK;
                    }
                    else if (string.Equals(request.RawUrl, "/health", StringComparison.OrdinalIgnoreCase) && string.Equals(request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                    {
                        var snapshot = await _healthRoot.HealthCheckRunner.ReadAsync();
                        await _healthRoot.OutputHealthFormatters
                            .OfType<HealthStatusTextOutputFormatter>()
                            .First()
                            .WriteAsync(output, snapshot);
                        response.StatusCode = (int) HttpStatusCode.OK;
                    }
                    else if (string.Equals(request.RawUrl, "/favicon.ico", StringComparison.OrdinalIgnoreCase)) { }
                    else
                    {
                        _logger.LogWarning("{Type}: Received BadRequest with the following url: {Url}", GetType().FullName, request.Url);

                        var responseString = "nope.exe";
                        var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                        response.ContentLength64 = buffer.Length;
                        output.Write(buffer, 0, buffer.Length);
                        response.StatusCode = (int) HttpStatusCode.BadRequest;
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "{Type}: Error received.", GetType().FullName);

                }
                finally
                {
                    output.Close();
                }
            }
        }
    }
}