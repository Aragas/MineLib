using System;

namespace MineLib.Protocol.Classic.Packets
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class PacketSizeAttribute : Attribute
    {
        public int Size { get; }

        public PacketSizeAttribute(int size) => Size = size;
    }
}