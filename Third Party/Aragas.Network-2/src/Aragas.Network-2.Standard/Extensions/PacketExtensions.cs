using Aragas.Network.IO;

using System;
using System.Net;

using static Aragas.Network.IO.PacketSerializer;
using static Aragas.Network.IO.PacketDeserializer;

namespace Aragas.Network.Extensions
{
    public static class PacketExtensions
    {
        public static void Init()
        {
            Extend<TimeSpan>(ReadTimeSpan, WriteTimeSpan);
            Extend<DateTime>(ReadDateTime, WriteDateTime);
            Extend<IPEndPoint>(ReadIPEndPoint, WriteIPEndPoint);
            Extend<Guid>(ReadGuid, WriteGuid);
        }

        private static void Extend<T>(Func<PacketDeserializer, int, T> readFunc, Action<PacketSerializer, T, bool> writeAction)
        {
            ExtendRead(readFunc);
            ExtendWrite(writeAction);
        }

        private static void WriteTimeSpan(PacketSerializer serializer, TimeSpan value, bool writeDefaultLength = true) => serializer.Write(value.Ticks);
        private static TimeSpan ReadTimeSpan(PacketDeserializer deserializer, int length = 0) => new TimeSpan(deserializer.Read<long>());

        private static void WriteDateTime(PacketSerializer serializer, DateTime value, bool writeDefaultLength = true) => serializer.Write(value.Ticks);
        private static DateTime ReadDateTime(PacketDeserializer deserializer, int length = 0) => new DateTime(deserializer.Read<long>());

        private static void WriteIPEndPoint(PacketSerializer serializer, IPEndPoint value, bool writeDefaultLength = true)
        {
            serializer.Write(value.Address.GetAddressBytes());
            serializer.Write(value.Port);
        }
        private static IPEndPoint ReadIPEndPoint(PacketDeserializer deserializer, int length = 0)
        {
            return new IPEndPoint(new IPAddress(deserializer.Read<byte[]>()), deserializer.Read<int>());
        }

        private static void WriteGuid(PacketSerializer serializer, Guid value, bool writeDefaultLength = true) => serializer.Write(value.ToByteArray(), false);
        private static Guid ReadGuid(PacketDeserializer deserializer, int length = 0) => new Guid(deserializer.Read<byte[]>(Array.Empty<byte>(), 16));
    }
}