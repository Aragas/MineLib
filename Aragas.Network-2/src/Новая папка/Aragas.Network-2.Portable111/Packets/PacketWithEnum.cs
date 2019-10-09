using System;
using System.Linq;

using Aragas.Network.IO;

namespace Aragas.Network.Packets
{
    /// <summary>
    /// A <see cref="Packet"/> with <see langword="int"/> <seealso cref="Packet{TPacketType, TReader, TWriter}.ID"/>. Must have an <seealso cref="System.Enum"/>.
    /// </summary>
    /// <typeparam name="TDeserializer"><see cref="StandardDeserialiser"/>. You can create a custom one or use <see cref="ProtobufDeserializer"/> and <see cref="ProtobufDeserializer"/></typeparam>
    /// <typeparam name="TSerializer"><see cref="StandardSerializer"/>. You can create a custom one or use <see cref="ProtobufSerializer"/> and <see cref="System.Enum"/></typeparam>
    /// <typeparam name="TEnumType">Any integer type. See <see cref="byte"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, any <see cref="Aragas.Network.Data.Variant"/></typeparam>
    public abstract class PacketWithEnum<TEnum, TNumberType, TSerializer, TDeserializer> : Packet<TNumberType, TSerializer, TDeserializer> 
        where TEnum : Enum where TSerializer : PacketSerializer where TDeserializer : PacketDeserializer
    {
        public override TNumberType ID { get; }

        protected PacketWithEnum() => ID = (TNumberType) (dynamic) Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Single(@enum => GetType().Name == $"{@enum}Packet");
    }
}