using Aragas.Network.Packets;

namespace Aragas.Network.IO
{
    public class StandardTransmission<TStandardPacketType> : SocketPacketTransmission<TStandardPacketType, int, ProtobufSerializer, ProtobufDeserializer> 
        where TStandardPacketType : Packet<int>
    {
        //public StandardTransmission(Socket socket, Stream socketStream = null, BasePacketFactory<TStandardPacketType, int, ProtobufSerializer, ProtobufDeserializer> factory = null)
        //    : base(socket, socketStream, factory) { }
    }
}