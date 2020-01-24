using Aragas.Network.IO;

using System;
using System.IO;

namespace MineLib.Server.Proxy.Protocol.Netty
{
    public sealed class LegacyPingSupportProtobufDeserializer : ProtobufDeserializer
    {
        public LegacyPingSupportProtobufDeserializer(Stream stream) : base(stream) { }

        protected override void Initialize(Stream stream)
        {
            Stream = stream;

            // Unsafe. 
            var dataLength = Read<byte>();
            if (dataLength == 0xFE)
            {
                // ?.? < 1.3 - FE
                // 1.4 < 1.5 - FE 01
                // 1.6 - FE 01 (2 + 11*2 + Read<short>)

                var payload = Read<byte>();
                if (payload == 0x01)
                {
                    var identifier = Read<byte>();
                    if (identifier == 0xFA)
                    {
                        var data1 = Read<byte[]>(null!, 24);
                        var data2Len = Read<short>();
                        var data2 = Read<byte[]>(null!, data2Len);

                        Span<byte> data = new byte[1 + 1 + 1 + data1.Length + 2 + data2.Length];
                        data[0] = dataLength;
                        data[1] = payload;
                        data[2] = identifier;
                        data1.CopyTo(data.Slice(3, 27));
                        BitConverter.GetBytes(data2Len).CopyTo(data.Slice(27, 2));
                        data2.CopyTo(data.Slice(29));

                        Stream = new MemoryStream(data.ToArray());
                    }
                    else
                        Stream = new MemoryStream(new byte[] { 0xFE, 0x01 });
                }
                else
                    Stream = new MemoryStream(new byte[] { 0xFE });
            }
            else
            {
                var data = ReadByteArray(dataLength);
                Stream = new MemoryStream(data);
            }
        }
    }
}