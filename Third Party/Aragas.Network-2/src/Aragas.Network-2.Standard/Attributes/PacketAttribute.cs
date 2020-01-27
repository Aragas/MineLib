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
    public sealed class PacketIDAttribute : Attribute
    {
        public object ID { get; }

        public PacketIDAttribute(object id) => ID = id;
    }
}