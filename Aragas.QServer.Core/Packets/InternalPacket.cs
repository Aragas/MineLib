using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using System;

namespace Aragas.QServer.Core.Packets
{
    public abstract class InternalPacket : PacketWithAttribute<VarInt>
    {
        // Because the messages are broadcasted, and received by everyone, they should have some kind of ID system.
        public Guid GUID;

        public override void Deserialize(IPacketDeserializer deserializer) => GUID = deserializer.Read(GUID);

        public override void Serialize(IPacketSerializer serializer) => serializer.Write(GUID);
    }
}