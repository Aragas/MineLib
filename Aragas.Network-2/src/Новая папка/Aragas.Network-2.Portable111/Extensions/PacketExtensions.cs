using System;

using Aragas.Network.IO;

using static Aragas.Network.IO.PacketSerializer;
using static Aragas.Network.IO.PacketDeserializer;

namespace Aragas.Network.Extensions
{
    public static class PacketExtensions
    {
        private static void Extend<T>(Func<PacketDeserializer, int, T> readFunc, Action<PacketSerializer, T, bool> writeAction)
        {
            ExtendRead(readFunc);
            ExtendWrite(writeAction);
        }

        public static void Init()
        {
            Extend<TimeSpan>(ReadTimeSpan, WriteTimeSpan);
            Extend<DateTime>(ReadDateTime, WriteDateTime);
        }

        private static void WriteTimeSpan(PacketSerializer serializer, TimeSpan value, bool writeDefaultLength = true) => serializer.Write(value.Ticks);
        private static TimeSpan ReadTimeSpan(PacketDeserializer deserializer, int length = 0) => new TimeSpan(deserializer.Read<long>());

        private static void WriteDateTime(PacketSerializer serializer, DateTime value, bool writeDefaultLength = true) => serializer.Write(value.Ticks);
        private static DateTime ReadDateTime(PacketDeserializer deserializer, int length = 0) => new DateTime(deserializer.Read<long>());
    }
}