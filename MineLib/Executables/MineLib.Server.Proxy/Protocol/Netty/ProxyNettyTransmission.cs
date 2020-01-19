using Aragas.Network.Data;
using Aragas.Network.IO;

using Microsoft.Extensions.Options;

using MineLib.Protocol.Netty;
using MineLib.Protocol.Netty.Packets.Server.Handshake;
using MineLib.Protocol.Netty.Protocol;
using MineLib.Protocol.Packets;
using MineLib.Server.Proxy.Data;

using System.Collections.Concurrent;
using System.Net.Sockets;

namespace MineLib.Server.Proxy.Protocol.Netty
{
    /// <summary>
    /// This class is not related and should not be related to MineLib.Protocol.* classes
    /// </summary>
    internal sealed class ProxyNettyTransmission : ProtobufTransmission<MinecraftPacket>
    {
        public ConcurrentQueue<byte[]> DataToSend { get; } = new ConcurrentQueue<byte[]>();

        public MineLibOptions MineLibOptions { get; set; }
        public VarInt ProtocolVersion { get; set; }
        public State State { get; set; } = State.Handshake;

        private ServerHandshakeFactory HandshakeFactory { get; } = new ServerHandshakeFactory();
        private ServerStatusFactory StatusFactory { get; } = new ServerStatusFactory();

        public ProxyNettyTransmission(Socket socket, IOptions<MineLibOptions> mineLibOptions) : base()
        {
            Socket = socket;
            MineLibOptions = mineLibOptions.Value;
        }

        public override MinecraftPacket? ReadPacket()
        {
            if (Socket.Available > 0)
            {
                if (State == State.Login || State == State.Play)
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
                MinecraftPacket? packet = State switch
                {
                    State.Handshake => HandshakeFactory.Create(id),
                    State.Status => StatusFactory.Create(id),

                    _ => null,
                };
                if (packet != null)
                {
                    packet.Deserialize(deserializer);

                    if (packet is HandshakePacket handshakePacket)
                    {
                        State = (State) (byte) handshakePacket.NextState;
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