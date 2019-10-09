using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using System;

namespace MineLib.Server.Core.Packets
{
    public abstract class InternalPacket : PacketWithAttribute<VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        // Because the messages are broadcasted, and received by everyone, they should have some kind of ID system.
        public Guid GUID;

        public override void Deserialize(ProtobufDeserializer deserializer) => GUID = deserializer.Read(GUID);

        public override void Serialize(ProtobufSerializer serializer) => serializer.Write(GUID);
    }
}