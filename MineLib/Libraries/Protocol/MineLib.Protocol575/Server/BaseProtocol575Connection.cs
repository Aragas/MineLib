﻿using Aragas.Network.Packets;

using MineLib.Protocol.Server;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MineLib.Protocol575.Server
{
    public abstract class BaseProtocol575Connection : ProtocolConnection
    {
        public override string Name => "Netty";
        public override string Version => "1.15.1";
        public override int NetworkVersion => 575;

#if DEBUG
        protected List<Packet> PacketsReceived { get; } = new List<Packet>();
        protected List<Packet> PacketsSended { get; } = new List<Packet>();
        protected List<Packet> PluginMessage { get; } = new List<Packet>();

        protected IEnumerable<Packet> LastPackets => PacketsReceived.Skip(Math.Max(0, PacketsReceived.Count - 50)) ?? Array.Empty<Packet>();
        protected Packet? LastPacket => PacketsReceived.Count > 0 ? PacketsReceived[PacketsReceived.Count - 1] : null;
#endif
    }
}