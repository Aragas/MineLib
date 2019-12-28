using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.QServer.Core;

using PokeD.Server.Proxy.Packets.P3D;

using System;
using System.IO;
using System.Net.Sockets;
using System.Reflection;

namespace PokeD.Server.Proxy.Protocol.P3D
{
    internal sealed class PlayerP3DConnection : DefaultConnectionHandler<ProxyP3DTransmission, ProxyP3DPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        /// <summary>
        /// For internal use only.
        /// </summary>
        public PlayerP3DConnection() { }
        public PlayerP3DConnection(Socket socket) : base(socket) { }

        protected override void HandlePacket(ProxyP3DPacket nettyPacket)
        {
            /*
            switch (nettyPacket)
            {
                case RequestPacket _:
                    SendPacket(new ResponsePacket()
                    {
                        JSONResponse = GetJSONResponse()
                    });
                    break;

                case PingPacket packet:
                    SendPacket(new PongPacket() { Time = packet.Time });
                    Task.Run(Disconnect);
                    break;
            }
            */
        }

        private static string GetFavicon()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MineLib.Server.Proxy.logo-1.png");
            if (stream != null)
            {
                using var ms = new MemoryStream();
                stream.CopyTo(ms);
                var png = ms.ToArray();
                var base64Png = Convert.ToBase64String(png);
                return $"data:image/png;base64,{base64Png}";
            }
            else
                return string.Empty;
        }

        protected override void AdditionalWork() => Stream.DoProxyIO();
    }
}