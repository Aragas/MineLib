using System.IO;
using System.Net.Sockets;

using Aragas.Network.Packets;

namespace Aragas.Network.IO
{
    public class StandardTransmission<TStandardPacketType> : SocketPacketTransmission<TStandardPacketType, int, ProtobufSerializer, ProtobufDeserializer> 
        where TStandardPacketType : Packet<int, ProtobufSerializer, ProtobufDeserializer>
    {
        public StandardTransmission(Socket socket, Stream socketStream = null, BasePacketFactory<TStandardPacketType, int, ProtobufSerializer, ProtobufDeserializer> factory = null)
            : base(socket, socketStream, factory) { }
    }
}