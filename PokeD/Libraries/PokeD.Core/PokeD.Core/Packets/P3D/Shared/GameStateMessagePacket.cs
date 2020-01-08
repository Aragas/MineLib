using Aragas.Network.Attributes;

using PokeD.Core.IO;

namespace PokeD.Core.Packets.P3D.Shared
{
    [Packet((int) P3DPacketTypes.GameStateMessage)]
    public class GameStateMessagePacket : P3DPacket
    {
        public string EventMessage { get => DataItems[0]; set => DataItems[0] = value; } 

        public override void Deserialize(P3DDeserializer deserializer) { }
        public override void Serialize(P3DSerializer serializer) { }
    }
}
