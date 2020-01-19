using Aragas.Network.IO;

namespace Aragas.Network.Packets
{
    /// <summary>
    /// Base class.
    /// </summary>
    public abstract class Packet { }

    /// <summary>
    /// A <see cref="Packet"/> with <see langword="int"/> <seealso cref="Packet{TPacketType, TReader, TWriter}.ID"/>. Must have an <seealso cref="Aragas.Network.Attributes.PacketAttribute"/>.
    /// </summary>
    /// <typeparam name="TIDType"><see cref="Packet"/>'s unique ID type. It will be used to differentiate <see cref="Packet"/>'s</typeparam>
    /// <typeparam name="TSerializer"><see cref="PacketSerializer"/>. You can create a custom one or use <see cref="StandardSerializer"/> and <see cref="ProtobufSerializer"/></typeparam>
    /// <typeparam name="TDeserializer"><see cref="PacketDeserializer"/>. You can create a custom one or use <see cref="StandardDeserializer"/> and <see cref="ProtobufDeserializer"/></typeparam>
    public abstract class Packet<TIDType> : Packet
    {
        public abstract TIDType ID { get; }

        public abstract void Deserialize(IPacketDeserializer deserializer);

        public abstract void Serialize(IStreamSerializer serializer);
    }
}