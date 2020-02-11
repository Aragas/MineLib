using Aragas.Network.IO;

using MineLib.Server.Proxy.Data;

using System;
using System.Text;

using static Aragas.Network.IO.PacketSerializer;
using static Aragas.Network.IO.PacketDeserializer;

namespace MineLib.Server.Proxy.Extensions
{
    public static class PacketExtensions
    {
        public static void Init()
        {
            Extend<UTF16BEString>(ReadUTF16BEString, WriteUTF16BEString);
        }

        private static void Extend<T>(Func<PacketDeserializer, int, T> readFunc, Action<PacketSerializer, T, bool> writeAction)
        {
            ExtendRead(readFunc);
            ExtendWrite(writeAction);
        }

        private static void WriteUTF16BEString(PacketSerializer serializer, UTF16BEString value, bool writeDefaultLength = true)
        {
            var data = Encoding.BigEndianUnicode.GetBytes(value);

            if(writeDefaultLength)
                serializer.Write((short) (data.Length / 2));

            serializer.Write(data, false);
        }
        private static UTF16BEString ReadUTF16BEString(PacketDeserializer deserializer, int length = 0)
        {
            if(length == 0)
                length = deserializer.Read<short>() * 2;

            return new UTF16BEString(Encoding.BigEndianUnicode.GetString(deserializer.Read<byte[]>(length: length)));
        }
    }
}
