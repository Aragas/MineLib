using Aragas.Network.Packets;

using MineLib.Protocol.Server;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MineLib.Protocol.Classic.Server
{
    public abstract class BaseProtocolClassicConnection : ProtocolConnection
    {
        public override string Name => "Classic";
        public override string Version => "0.0.0.0";
        public override int NetworkVersion => 0;

#if DEBUG
        protected List<Packet> PacketsReceived { get; } = new List<Packet>();
        protected List<Packet> PacketsSended { get; } = new List<Packet>();
        protected List<Packet> PluginMessage { get; } = new List<Packet>();

        protected IEnumerable<Packet> LastPackets => PacketsReceived.Skip(Math.Max(0, PacketsReceived.Count - 50)) ?? Array.Empty<Packet>();
        protected Packet? LastPacket => PacketsReceived.Count > 0 ? PacketsReceived[PacketsReceived.Count - 1] : null;
#endif
    }
}