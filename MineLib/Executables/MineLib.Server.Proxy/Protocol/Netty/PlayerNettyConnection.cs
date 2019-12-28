using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.QServer.Core;

using MineLib.Server.Proxy.Packets.Netty;
using MineLib.Server.Proxy.Packets.Netty.Clientbound;
using MineLib.Server.Proxy.Packets.Netty.Serverbound;

using System;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;

namespace MineLib.Server.Proxy.Protocol.Netty
{
    internal sealed class PlayerNettyConnection : DefaultConnectionHandler<ProxyNettyTransmission, ProxyNettyPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        /// <summary>
        /// For internal use only.
        /// </summary>
        public PlayerNettyConnection() { }
        public PlayerNettyConnection(Socket socket) : base(socket) { }

        protected override void HandlePacket(ProxyNettyPacket packet)
        {
            switch (packet)
            {
                case RequestPacket _:
                    SendPacket(new ResponsePacket()
                    {
                        JSONResponse = GetJSONResponse()
                    });
                    break;

                case PingPacket pingPacket:
                    SendPacket(new PongPacket() { Time = pingPacket.Time });
                    Task.Run(Disconnect);
                    break;
            }
        }

        private static string GetJSONResponse() => @$"
{{
    ""version"":
    {{
        ""name"": ""Any Version"",
        ""protocol"": 0
    }},
    ""players"":
    {{
        ""max"": {Program.MaxConnections},
        ""online"": {Program.CurrentConnections}
    }},	
    ""description"": 
    {{
        ""text"": ""{Program.Description}""
    }},
    ""favicon"": ""{GetFavicon()}"",
    ""modinfo"":
    {{
        ""type"": ""FML"",
        ""modList"": []
    }}
}}
";

        private static string GetFavicon()
        {
            using var ms = new MemoryStream();
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MineLib.Server.Proxy.logo-1.png");
            if (stream != null)
            {
                stream.CopyTo(ms);
                var png = ms.ToArray();
                var base64Png = Convert.ToBase64String(png);
                return $"data:image/png;base64,{base64Png}";
            }
            return string.Empty;
        }

        protected override void AdditionalWork() => Stream.DoProxyIO();
    }
}