using System;

namespace Aragas.Network.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PacketAttribute : Attribute
    {
        public object ID { get; }

        public PacketAttribute(object id) => ID = id;
    }
}