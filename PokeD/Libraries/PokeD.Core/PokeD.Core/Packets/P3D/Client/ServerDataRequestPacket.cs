using Aragas.Network.Attributes;

using PokeD.Core.IO;

namespace PokeD.Core.Packets.P3D.Client
{
    [Packet((int) P3DPacketTypes.ServerDataRequest)]
    public class ServerDataRequestPacket : P3DPacket
    {
        public string DontEvenKnow { get => DataItems[0]; set => DataItems[0] = value; }

        public override void Deserialize(P3DDeserializer deserializer) { }
        public override void Serialize(P3DSerializer serializer) { }
    }
}
