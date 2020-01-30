using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

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

    [ApiController, Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ClassicServersDbContext _classicServers;
        private readonly ILogger _logger;

        public ApiController(ClassicServersDbContext classicServers, ILogger<ApiController> logger)
        {
            _classicServers = classicServers;
            _logger = logger;
        }

        [HttpGet, Route("server")]
        public ActionResult Server()
        {
            return Content("");
        }

        [HttpGet, Route("servers")]
        public string Servers()
        {
            var query = _classicServers.Servers
                .AsEnumerable()
                .Where(s => DateTimeOffset.UtcNow < s.LastUpdate + TimeSpan.FromMinutes(2))
                .Select(s =>
                {
                    var ip = IPAddress.Parse(s.IP);
                    s.IP = ip.IsIPv4MappedToIPv6 ? ip.MapToIPv4().ToString() : ip.MapToIPv6().ToString();
                    return s;
                });

            return JsonConvert.SerializeObject(query, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new ShouldSerializeContractResolver(User.Identity.IsAuthenticated),
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}