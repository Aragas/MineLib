using Aragas.Network.IO;

using System;

using static Aragas.Network.IO.PacketSerializer;
using static Aragas.Network.IO.PacketDeserializer;

namespace MineLib.Server.Core.Extensions
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

        }
    }
}