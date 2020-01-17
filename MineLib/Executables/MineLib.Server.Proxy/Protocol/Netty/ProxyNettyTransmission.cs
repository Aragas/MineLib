using Aragas.Network.Data;
using Aragas.Network.IO;

using Microsoft.Extensions.Options;

using MineLib.Server.Proxy.Data;
using MineLib.Server.Proxy.Protocol.Netty.Factory;
using MineLib.Server.Proxy.Protocol.Netty.Packets;
using MineLib.Server.Proxy.Protocol.Netty.Packets.Serverbound;

using System.Collections.Concurrent;
using System.Net.Sockets;

namespace MineLib.Server.Proxy.Protocol.Netty
{
    /// <summary>
    /// This class is not related and should not be related to MineLib.Protocol.* classes
    /// </summary>
    internal sealed class ProxyNettyTransmission : ProtobufTransmission<ProxyNettyPacket>
    {
        public ConcurrentQueue<byte[]> DataToSend { get; } = new ConcurrentQueue<byte[]>();

        public MineLibOptions MineLibOptions { get; set; }
        public VarInt ProtocolVersion { get; set; }
        public Data.State State { get; set; } = Data.State.Handshake;

        private HandshakeStateFactory HandshakeFactory { get; } = new HandshakeStateFactory();
        private StatusStateFactory StatusFactory { get; } = new StatusStateFactory();

        public ProxyNettyTransmission(Socket socket, IOptions<MineLibOptions> mineLibOptions) : base()
        {
            Socket = socket;
            MineLibOptions = mineLibOptions.Value;
        }

        public override ProxyNettyPacket? ReadPacket()
        {
            if (Socket.Available > 0)
            {
                if (State == Data.State.Login || State == Data.State.Play)
                {
                    var buffer = new byte[Socket.Available];
                    Socket.Receive(buffer);
                    DataToSend.Enqueue(buffer);
                    return null;
                }

                using var deserializer = MineLibOptions.NettyLegacyPingEnable
                    ? new LegacyPingSupportProtobufDeserializer(Stream)
                    : new ProtobufDeserializer(Stream);
                var id = MineLibOptions.NettyLegacyPingEnable
                    ? deserializer.Read<byte>()
                    : deserializer.Read<VarInt>();
                ProxyNettyPacket? packet = State switch
                {
                    Data.State.Handshake => HandshakeFactory.Create(id),
                    Data.State.Status => StatusFactory.Create(id),

                    _ => null,
                };
                if (packet != null)
                {
                    packet.Deserialize(deserializer);

                    if (packet is HandshakePacket handshakePacket)
                    {
                        State = (Data.State)(byte) handshakePacket.NextState;
                        ProtocolVersion = handshakePacket.ProtocolVersion;
                    }

                    return packet;
                }
            }

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DataToSend.Clear();
            }

            base.Dispose(disposing);
        }
    }
}