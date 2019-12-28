using System;

namespace Aragas.Network.Attributes
{
    // TODO: Generic Attribute
    /*
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class PacketAttribute<TIDType> : Attribute
    {
        public TIDType ID { get; }

        public PacketAttribute(TIDType id) => ID = id;
    }
    */
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class PacketAttribute : Attribute
    {
        public object ID { get; }

        public PacketAttribute(object id) => ID = id;
    }
}