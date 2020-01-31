using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MineLib.Server.Heartbeat.Infrastructure.Data;

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MineLib.Server.Heartbeat.Controllers
{
    [ApiController, Route("[controller]")]
    public sealed class ServerController : ControllerBase
    {
        private readonly IClassicServersRepository _classicServersRepository;
        private readonly ILogger _logger;

        public ServerController(IClassicServersRepository classicServersRepository, ILogger<ServerController> logger)
        {
            _classicServersRepository = classicServersRepository;
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
            _logger.LogInformation("{Type}: Received /hearthbeat with url ({DisplayUrl})", GetType().FullName, Request.GetDisplayUrl());

            var ip = HttpContext.Connection.RemoteIpAddress.ToString();

            using var md5 = MD5.Create();
            var hash = string.Concat(md5.ComputeHash(Encoding.UTF8.GetBytes($"{ip}:{port}")).Select(x => x.ToString("x2")));

            var server = _classicServersRepository.GetByHash(hash);
            if (server == null)
            {
                _classicServersRepository.Add(new ClassicServer()
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
                    IsSupportingWeb = isSupportingWeb,
                    LastUpdate = DateTimeOffset.UtcNow,
                    Added = DateTimeOffset.UtcNow
                });
            }
            else
            {
                server.LastUpdate = DateTimeOffset.UtcNow;
                _classicServersRepository.Update(server);
            }

            return Content($"{Request.Scheme}://{Request.Host}/server/play/{hash}");
        }

        [HttpGet, HttpPost, Route("play/{hash}")]
        public ActionResult Play(string hash)
        {
            _logger.LogInformation("{Type}: Received /play with url ({DisplayUrl})", GetType().FullName, Request.GetDisplayUrl());

            return Content("Not implemented yet!");
        }
    }
}