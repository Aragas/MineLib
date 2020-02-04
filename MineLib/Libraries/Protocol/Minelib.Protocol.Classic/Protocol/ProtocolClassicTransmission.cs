﻿using Aragas.Network.IO;
using Aragas.QServer.Core.NetworkBus;
using Aragas.QServer.Core.Protocol;

using MineLib.Protocol.Classic.Packets;
using MineLib.Protocol.Classic.Protocol.Factory;

using System;

namespace MineLib.Protocol.Classic.Protocol
{
    public class ProtocolClassicTransmission : SocketPacketINetworkBusTransmission<ClassicPacket, byte, StandardSerializer, StandardDeserializer>
    {
        public new ClientClassicFactory Factory = new ClientClassicFactory();

        public ProtocolClassicTransmission(IAsyncNetworkBus networkBus, Guid playerId) : base(networkBus, playerId) { }

        public override ClassicPacket? ReadPacket()
        {
            var data = Receive(0);
            if (data.IsEmpty)
                return null;

            using var deserializer = new ProtobufDeserializer(data);
            var id = deserializer.Read<byte>();
            var packet = Factory.Create(id);
            if (packet != null)
            {
                packet.Deserialize(deserializer);

                return packet;
            }

            return null;
        }

        public override void SendPacket(ClassicPacket packet)
        {
            base.SendPacket(packet);
        }
    }
}
