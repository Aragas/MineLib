using Aragas.Network.Data;
using Aragas.Network.Packets;

namespace Aragas.Network.IO
{
    public class ProtobufTransmission<TProtobufPacketType> : SocketPacketTransmission<TProtobufPacketType, VarInt, ProtobufSerializer, ProtobufDeserializer> 
        where TProtobufPacketType : Packet<VarInt>
    {
        protected ProtobufTransmission() : base() { }
        //public ProtobufTransmission(Socket socket, Stream socketStream = null, BasePacketFactory<TProtobufPacketType, VarInt, ProtobufSerializer, ProtobufDeserializer> factory = null) 
        //    : base(socket, socketStream, factory) { }
    }
}