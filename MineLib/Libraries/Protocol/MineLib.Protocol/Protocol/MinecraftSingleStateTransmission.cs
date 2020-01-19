using Aragas.Network.IO;

using MineLib.Protocol.Packets;

namespace MineLib.Protocol.Protocol
{
    public class MinecraftSingleStateTransmission<TPacketType> : ProtobufTransmission<TPacketType> 
        where TPacketType : MinecraftEnumPacket
    {
        public MinecraftSingleStateTransmission() : base() { }
        //public MinecraftSingleStateTransmission(Socket socket, BasePacketFactory<TPacketType, VarInt, ProtobufSerializer, ProtobufDeserializer> factory = null) : base(socket, null, factory) { }

        public bool TryReadPacket(out MinecraftEnumPacket? packet)
        {
            packet = ReadPacket();
            return packet != null;
        }
    }
}