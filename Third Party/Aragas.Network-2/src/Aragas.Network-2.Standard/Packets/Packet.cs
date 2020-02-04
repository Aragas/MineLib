using Aragas.Network.IO;

namespace Aragas.Network.Packets
{
    /// <summary>
    /// Base class.
    /// </summary>
    public abstract class Packet { }

    /// <summary>
    /// A <see cref="Packet"/> with <see langword="int"/> <seealso cref="Packet{TPacketType}.ID"/>. Must have an <seealso cref="Aragas.Network.Attributes.PacketIDAttribute"/>.
    /// </summary>
    /// <typeparam name="TIDType"><see cref="Packet"/>'s unique ID type. It will be used to differentiate <see cref="Packet"/>'s</typeparam>
    public abstract class Packet<TIDType> : Packet
    {
        public abstract TIDType ID { get; }

        public abstract void Deserialize(IPacketDeserializer deserializer);

        public abstract void Serialize(IPacketSerializer serializer);
    }
}