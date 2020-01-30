using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MineLib.Server.Heartbeat.Controllers
{
    [ApiController, Route("[controller]")]
    public class ServerController : ControllerBase
    {
        private readonly ClassicServersDbContext _classicServers;
        private readonly ILogger _logger;

        public ServerController(ClassicServersDbContext classicServers, ILogger<ServerController> logger)
        {
            _classicServers = classicServers;
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
            _logger.LogInformation("{TypeName}: Received /hearthbeat with url ({DisplayUrl})", GetType().FullName, Request.GetDisplayUrl());

            string ip = HttpContext.Connection.RemoteIpAddress.ToString();
            //if (HttpContext.Connection.RemoteIpAddress.IsIPv4MappedToIPv6)
            //    ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            //else
            //    ip = HttpContext.Connection.RemoteIpAddress.MapToIPv6().ToString()

            using var md5 = MD5.Create();
            var hash = string.Concat(md5.ComputeHash(Encoding.UTF8.GetBytes($"{ip}:{port}")).Select(x => x.ToString("x2")));

            _classicServers.AddOrUpdate(new ClassicServer()
            {
                Name = name,
                IP = ip,
                Port = port,
                Hash = hash,
                Salt = salt,
                IsPublic = isPublic,
                Players = players,
                MaxPlayers = maxPlayers,
                Version = version,
                Software = software,
                IsSupportingWeb = isSupportingWeb
            });
            _classicServers.SaveChanges();

            return Content($"{Request.Scheme}://{Request.Host}/server/play/{hash}");
        }

        [HttpGet, HttpPost, Route("play/{hash}")]
        public ActionResult Play(string hash)
        {
            _logger.LogInformation("{TypeName}: Received /play with url ({DisplayUrl})", GetType().FullName, Request.GetDisplayUrl());

            return Content("Not implemented yet!");
        }
    }
}