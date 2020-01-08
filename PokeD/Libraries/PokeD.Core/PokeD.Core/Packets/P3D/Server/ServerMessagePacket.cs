using Aragas.Network.Attributes;

using PokeD.Core.IO;

namespace PokeD.Core.Packets.P3D.Server
{
    [Packet((int) P3DPacketTypes.ServerMessage)]
    public class ServerMessagePacket : P3DPacket
    {
        public string Message { get => DataItems[0]; set => DataItems[0] = value; }

        public override void Deserialize(P3DDeserializer deserializer) { }
        public override void Serialize(P3DSerializer serializer) { }
    }
}
