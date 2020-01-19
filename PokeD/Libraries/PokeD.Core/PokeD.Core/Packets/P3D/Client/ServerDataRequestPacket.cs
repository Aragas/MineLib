using Aragas.Network.Attributes;
using Aragas.Network.IO;

using PokeD.Core.Data.P3D;
using PokeD.Core.IO;

namespace PokeD.Core.Packets.P3D.Client
{
    [Packet((int) P3DPacketTypes.ServerDataRequest)]
    public class ServerDataRequestPacket : P3DPacket
    {
        public string DontEvenKnow { get => DataItems[0]; set => DataItems[0] = value; }

        public override void Deserialize(IPacketDeserializer deserializer) { }
        public override void Serialize(IStreamSerializer serializer) { }
    }
}
