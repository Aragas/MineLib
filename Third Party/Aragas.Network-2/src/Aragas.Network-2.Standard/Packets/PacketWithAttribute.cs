using Aragas.Network.Attributes;
using Aragas.Network.IO;
using System;
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
    public abstract class PacketWithIDAttribute<TIDType> : Packet<TIDType>
    {
        /// <summary>
        /// Basically Nullable without struct constraint
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Serializable]
        internal protected struct Initializable<T>
        {
            private readonly bool hasInitialized;  // Do not rename (binary serialization)
            private readonly T value; // Do not rename (binary serialization)

            public Initializable(T value)
            {
                this.value = value;
                hasInitialized = true;
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Simplification", "RCS1085:Use auto-implemented property.", Justification = "Binary serialization")]
            public bool HasInitialized
            {
                get => hasInitialized;
            }

            public T Value
            {
                get
                {
                    if (!hasInitialized)
                    {
                        throw new Exception();
                    }
                    return value;
                }
            }

            public T GetValueOrDefault() => value;

            public T GetValueOrDefault(T defaultValue) => hasInitialized ? value : defaultValue;

            public override bool Equals(object? other)
            {
                if (!hasInitialized) return other == null;
                if (other == null) return false;
                return value!.Equals(other);
            }

            public override int GetHashCode() => hasInitialized ? value!.GetHashCode() : 0;

            public override string? ToString() => hasInitialized ? value!.ToString() : "";

            public static implicit operator Initializable<T>(T value) => new Initializable<T>(value);

            public static explicit operator T(Initializable<T> value) => value!.Value;
        }

        private Initializable<TIDType> _id;
        public sealed override TIDType ID
        {
            get 
            {
                var id = _id.HasInitialized ? _id : (_id = (TIDType) (dynamic) GetType().GetCustomAttribute<PacketIDAttribute>().ID);
                return id.Value;
            }
        }
    }
}