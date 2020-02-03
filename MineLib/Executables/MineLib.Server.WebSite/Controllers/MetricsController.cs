/*
using App.Metrics;
using App.Metrics.Formatters.Prometheus;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Threading.Tasks;

namespace MineLib.Server.Heartbeat.Controllers
{
    [Route("")]
    public sealed class MetricsController : ControllerBase
    {
        private readonly IMetricsRoot _metricsRoot;
        private readonly ILogger _logger;

        public MetricsController(IMetricsRoot metricsRoot, ILogger<ApiController> logger)
        {
            _metricsRoot = metricsRoot;
            _logger = logger;
        }

        [HttpGet("/metrics")]
        public async Task<ActionResult> Metrics()
        {
            Response.ContentType = "text/plain";
            var snapshot = _metricsRoot.Snapshot.Get();
            await _metricsRoot.OutputMetricsFormatters
                .OfType<MetricsPrometheusTextOutputFormatter>()
                .First()
                .WriteAsync(Response.Body, snapshot);

            return Ok();
        }
    }
}
*/