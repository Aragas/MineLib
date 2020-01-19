using Aragas.Network.Attributes;
using Aragas.Network.IO;

using System.Reflection;

namespace Aragas.Network.Packets
{
    /*
    /// <summary>
    /// A <see cref="Packet"/> with <see langword="int"/> <seealso cref="Packet{TPacketType, TReader, TWriter}.ID"/>. Must have a <seealso cref="PacketAttribute"/>.
    /// </summary>
    /// <typeparam name="TDeserializer"><see cref="StreamDeserializer"/>. You can create a custom one or use <see cref="StandardDeserializer"/> and <see cref="ProtobufDeserializer"/></typeparam>
    /// <typeparam name="TSerializer"><see cref="StreamSerializer"/>. You can create a custom one or use <see cref="StandardSerializer"/> and <see cref="ProtobufSerializer"/></typeparam>
    /// <typeparam name="TIntegerType">Any integer type. See <see cref="byte"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, any <see cref="Aragas.Network.Data.Variant"/></typeparam>
    public abstract class PacketWithAttribute<TIDType, TSerializer, TDeserializer> : Packet<TIDType, TSerializer, TDeserializer> where TIDType : struct where TSerializer : PacketSerializer where TDeserializer : PacketDeserializer
    {
        private TIDType? _id;
        public sealed override TIDType ID => _id ?? (_id = (TIDType) (dynamic) GetType().GetCustomAttribute<PacketAttribute>().ID).GetValueOrDefault();
    }
    */

    /// <summary>
    /// A <see cref="Packet"/> with <see langword="int"/> <seealso cref="Packet{TPacketType, TReader, TWriter}.ID"/>. Must have a <seealso cref="PacketAttribute"/>.
    /// </summary>
    /// <typeparam name="TIDType">Any type used for <see cref="Packet"/> identification.</typeparam>
    /// <typeparam name="TSerializer"><see cref="StreamSerializer"/>. You can create a custom one or use <see cref="StandardSerializer"/> and <see cref="ProtobufSerializer"/></typeparam>
    /// <typeparam name="TDeserializer"><see cref="StreamDeserializer"/>. You can create a custom one or use <see cref="StandardDeserializer"/> and <see cref="ProtobufDeserializer"/></typeparam>
    public abstract class PacketWithAttribute<TIDType> : Packet<TIDType>
    {
        private Initializable<TIDType> _id;
        public sealed override TIDType ID
        {
            get 
            {
                var t = _id.HasInitialized
                ? _id
                : (_id = (TIDType) (dynamic) GetType().GetCustomAttribute<PacketAttribute>().ID);
                return t.Value;
            }
        }
    }
}