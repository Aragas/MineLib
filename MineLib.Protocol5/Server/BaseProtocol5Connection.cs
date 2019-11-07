using Aragas.Network.Packets;

using MineLib.Protocol.Server;
using MineLib.Server.Core;

using System.Collections.Generic;

namespace MineLib.Protocol5.Server
{
    public abstract class BaseProtocol5Connection : ProtocolConnection
    {
        public override string Name => "Netty";
        public override string Version => "1.7.10";
        public override int NetworkVersion => 5;

        // Workaround as Protocol is inside PlayerBus
        private static IMBus? _playerBus;
        public static IMBus PlayerBus => _playerBus ?? (_playerBus = new NetworkMBus($"{InternalBus.Host}/minelib/server/playerbus"));

#if DEBUG
        protected List<Packet> PacketsReceived { get; } = new List<Packet>();
        protected List<Packet> PacketsSended { get; } = new List<Packet>();
        protected List<Packet> PluginMessage { get; } = new List<Packet>();

        protected List<Packet>? LastPackets => PacketsReceived?.GetRange(PacketsReceived.Count - 50, 50);
        protected Packet? LastPacket => PacketsReceived.Count > 0 ? PacketsReceived[PacketsReceived.Count - 1] : null;
#endif
    }
}