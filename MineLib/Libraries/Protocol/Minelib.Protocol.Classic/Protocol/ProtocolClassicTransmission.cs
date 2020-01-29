using Aragas.Network.IO;
using Aragas.QServer.Core.Protocol;

using MineLib.Protocol.Classic.Packets;
using MineLib.Protocol.Classic.Protocol.Factory;

namespace MineLib.Protocol.Classic.Protocol
{
    public class ProtocolClassicTransmission : SocketPacketINetworkBusTransmission<ClassicPacket, byte, StandardSerializer, StandardDeserializer>
    {
        private ClientClassicFactory Factory { get; } = new ClientClassicFactory();

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
