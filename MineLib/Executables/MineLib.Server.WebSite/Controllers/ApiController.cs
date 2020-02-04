using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MineLib.Server.Heartbeat.Infrastructure.Data;

using System;
using System.Linq;
using System.Net;

namespace MineLib.Server.Heartbeat.Controllers
{
    public sealed class ApiController : Controller
    {
        private readonly IClassicServersRepository _classicServerRepository;
        private readonly ILogger _logger;

        public ApiController(IClassicServersRepository classicServerRepository, ILogger<ApiController> logger)
        {
            _classicServerRepository = classicServerRepository ?? throw new ArgumentNullException(nameof(classicServerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{data}")]
        public ActionResult Server(string data)
        {
            _logger.LogInformation("{Type}: Received /api/server/{data} request", GetType().FullName, data);

            var hashes = data.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if(hashes.Length == 0)
                return Content("");

            var servers = _classicServerRepository.List()
                .Where(s => hashes.Contains(s.Hash) && DateTimeOffset.UtcNow < s.LastUpdate + TimeSpan.FromMinutes(2))
                .Select(s =>
                {
                    var ip = IPAddress.Parse(s.IP);
                    s.IP = ip.IsIPv4MappedToIPv6 ? ip.MapToIPv4().ToString() : ip.MapToIPv6().ToString();
                    return s;
                });

            return View(servers);
        }

        public IActionResult Servers()
        {
            _logger.LogInformation("{Type}: Received /api/servers request", GetType().FullName);

            var servers = _classicServerRepository.List()
                .Where(s => DateTimeOffset.UtcNow < s.LastUpdate + TimeSpan.FromMinutes(2))
                .Select(s =>
                {
                    var ip = IPAddress.Parse(s.IP);
                    s.IP = ip.IsIPv4MappedToIPv6 ? ip.MapToIPv4().ToString() : ip.MapToIPv6().ToString();
                    return s;
                });

            return View(servers);
        }
    }
}