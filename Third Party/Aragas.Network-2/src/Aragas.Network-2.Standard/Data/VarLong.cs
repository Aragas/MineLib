using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace Aragas.Network.Data
{
    /// <summary>
    /// Encoded Int64. Not optimal for negative values.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct VarLong : IEquatable<VarLong>
    {
        public int Size => Variant.VariantSize(_value);


        private readonly ulong _value;


        public VarLong(ulong value) { _value = value; }
        public VarLong(long value) { _value = (ulong) value; }


        public byte[] Encode() => Encode(this);


        public override string ToString() => _value.ToString(CultureInfo.InvariantCulture);

        public static VarLong Parse(string str) => new VarLong(ulong.Parse(str, CultureInfo.InvariantCulture));

        public static byte[] Encode(VarLong value) => Variant.Encode(value._value);
        public static int Encode(VarLong value, byte[] buffer, int offset)
        {
            return Variant.Encode(buffer, offset, value._value);
            //var encoded = value.Encode();
            //Array.Copy(encoded, 0, buffer, offset, encoded.Length);
            //return encoded.Length;
        }
        public static int Encode(VarLong value, Stream stream)
        {
            return Variant.Encode(stream, value._value);
            //var encoded = value.Encode();
            //stream.Write(encoded, 0, encoded.Length);
            //return encoded.Length;
        }

        public static VarLong Decode(in ReadOnlySpan<byte> buffer) => new VarLong(Variant.Decode(in buffer));
        public static VarLong Decode(byte[] buffer, int offset) => new VarLong(Variant.Decode(buffer, offset));
        public static VarLong Decode(Stream stream) => new VarLong(Variant.Decode(stream));
        public static int Decode(byte[] buffer, int offset, out VarLong result)
        {
            result = Decode(buffer, offset);
            return result.Size;
        }
        public static int Decode(Stream stream, out VarLong result)
        {
            result = Decode(stream);
            return result.Size;
        }


        public static explicit operator VarLong(ushort value) => new VarLong(value);
        public static explicit operator VarLong(uint value) => new VarLong(value);
        public static explicit operator VarLong(ulong value) => new VarLong(value);

        public static implicit operator ushort(VarLong value) => (ushort) value._value;
        public static implicit operator uint(VarLong value) => (uint) value._value;
        public static implicit operator ulong(VarLong value) => value._value;
        public static implicit operator VarLong(Enum value) => new VarLong(Convert.ToUInt64(value, CultureInfo.InvariantCulture));


        public static bool operator !=(VarLong a, VarLong b) => !a.Equals(b);
        public static bool operator ==(VarLong a, VarLong b) => a.Equals(b);

        public bool Equals(VarLong other) => other._value.Equals(_value);
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((VarLong) obj);
        }
        public override int GetHashCode() => _value.GetHashCode();
    }
}