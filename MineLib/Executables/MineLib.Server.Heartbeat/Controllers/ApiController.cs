using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MineLib.Server.Heartbeat.Infrastructure.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace MineLib.Server.Heartbeat.Controllers
{
    public class ShouldSerializeContractResolver : DefaultContractResolver
    {
        private readonly bool _isVerified;
        public ShouldSerializeContractResolver(bool isVerified)
        {
            _isVerified = isVerified;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            property.ShouldSerialize = instance => member.Name switch
            {
                nameof(ClassicServer.LastUpdate) => false,
                nameof(ClassicServer.Added) => false,
                nameof(ClassicServer.IsPublic) => false,

                nameof(ClassicServer.IP) => _isVerified,
                nameof(ClassicServer.Port) => _isVerified,
                nameof(ClassicServer.Hash) => _isVerified,
                nameof(ClassicServer.Salt) => _isVerified,

                _ => true,
            };

            return property;
        }
    }

    //[ApiController, Route("[controller]")]
    public class ApiController : Controller
    {
        private readonly ClassicServersContext _classicServerRepository;
        private readonly ILogger _logger;

        public ApiController(ClassicServersContext classicServerRepository, ILogger<ApiController> logger)
        {
            _classicServerRepository = classicServerRepository;
            _logger = logger;
        }

        [HttpGet, Route("server")]
        public ActionResult Server()
        {
            return Content("");
        }

        public IActionResult Servers(
            //string sortOrder, string searchString
            )
        {
            var servers = _classicServerRepository.Servers
                .AsEnumerable()
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