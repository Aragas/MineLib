using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MineLib.Server.Heartbeat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ServerController> _logger;

        public ServerController(IConfiguration configuration, ILogger<ServerController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        // https://localhost:5001/server/Heartbeat
        [HttpGet, HttpPost, Route("heartbeat")]
        public ActionResult Heartbeat(
            [FromQuery(Name = "name")] string name,
            [FromQuery(Name = "port")] ushort port,
            [FromQuery(Name = "salt")] string salt,
            [FromQuery(Name = "users")] int players,
            [FromQuery(Name = "max")] int maxPlayers,
            [FromQuery(Name = "public")] bool isPublic,
            [FromQuery(Name = "version")] int? version,
            [FromQuery(Name = "software")] string? software,
            [FromQuery(Name = "web")] bool? isSupportingWeb)
        {
            var ip = HttpContext.Connection.RemoteIpAddress;

            using var db = new ClassicServersDbContext(_configuration);
            db.AddOrUpdate(new ClassicServer()
            {
                Name = name,
                IP = ip.ToString(),
                Port = port,
                Salt = salt,
                IsPublic = isPublic,
                Players = players,
                MaxPlayers = maxPlayers,
                Version = version,
                Software = software,
                IsSupportingWeb = isSupportingWeb
            });

            using var md5 = MD5.Create();
            var hash = string.Concat(md5.ComputeHash(Encoding.UTF8.GetBytes($"{ip}:{port}")).Select(x => x.ToString("x2")));

            return Content(hash);
        }
    }
}