using System.Collections.Generic;

using Aragas.Network.Packets;

using MineLib.Protocol.Server;

namespace MineLib.Protocol5.Server
{
    public abstract class BaseProtocol5Connection : ProtocolConnection
    {
        public override string Name => "Netty";
        public override string Version => "1.7.10";
        public override int NetworkVersion => 5;

#if DEBUG
        protected List<Packet> PacketsReceived { get; } = new List<Packet>();
        protected List<Packet> PacketsSended { get; } = new List<Packet>();
        protected List<Packet> PluginMessage { get; } = new List<Packet>();

        protected List<Packet> LastPackets => PacketsReceived?.GetRange(PacketsReceived.Count - 50, 50);
        protected Packet LastPacket => PacketsReceived[PacketsReceived.Count - 1];
#endif
    }
}