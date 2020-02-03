/*
using App.Metrics.Health;
using App.Metrics.Health.Formatters.Ascii;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Threading.Tasks;

namespace MineLib.Server.Heartbeat.Controllers
{
    [Route("")]
    public sealed class HealthController : ControllerBase
    {
        private readonly IHealthRoot _healthRoot;
        private readonly ILogger _logger;

        public HealthController(IHealthRoot healthRoot, ILogger<ApiController> logger)
        {
            _healthRoot = healthRoot;
            _logger = logger;
        }

        [HttpGet("/health")]
        public async Task<ActionResult> Health()
        {
            Response.ContentType = "text/plain";

            var snapshot = await _healthRoot.HealthCheckRunner.ReadAsync();
            await _healthRoot.OutputHealthFormatters
                .OfType<HealthStatusTextOutputFormatter>()
                .First()
                .WriteAsync(Response.Body, snapshot);

            return Ok();
        }
    }
}
*/