using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace Aragas.Network.Data
{
    /// <summary>
    /// Encoded Int16. Not optimal for negative values.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct VarShort : IEquatable<VarShort>
    {
        public int Size => Variant.VariantSize(_value);


        private readonly ushort _value;


        public VarShort(ushort value) { _value = value; }
        public VarShort(short value) { _value = (ushort) value; }


        public byte[] Encode() => Encode(this);


        public override string ToString() => _value.ToString(CultureInfo.InvariantCulture);

        public static VarShort Parse(string str) => new VarShort(ushort.Parse(str, CultureInfo.InvariantCulture));

        public static byte[] Encode(VarShort value) => Variant.Encode(value._value);
        public static int Encode(VarShort value, byte[] buffer, int offset)
        {
            return Variant.Encode(buffer, offset, value._value);
            //var encoded = value.Encode();
            //Array.Copy(encoded, 0, buffer, offset, encoded.Length);
            //return encoded.Length;
        }
        public static int Encode(VarShort value, Stream stream)
        {
            return Variant.Encode(stream, value._value);
            //var encoded = value.Encode();
            //stream.Write(encoded, 0, encoded.Length);
            //return encoded.Length;
        }

        public static VarShort Decode(in ReadOnlySpan<byte> buffer) => new VarShort((ushort) Variant.Decode(in buffer));
        public static VarShort Decode(byte[] buffer, int offset) => new VarShort((ushort) Variant.Decode(buffer, offset));
        public static VarShort Decode(Stream stream) => new VarShort((ushort) Variant.Decode(stream));
        public static int Decode(byte[] buffer, int offset, out VarShort result)
        {
            result = Decode(buffer, offset);
            return result.Size;
        }
        public static int Decode(Stream stream, out VarShort result)
        {
            result = Decode(stream);
            return result.Size;
        }


        public static explicit operator VarShort(ushort value) => new VarShort(value);

        public static implicit operator ushort(VarShort value) => value._value;
        public static implicit operator uint(VarShort value) => value._value;
        public static implicit operator ulong(VarShort value) => value._value;
        public static implicit operator VarShort(Enum value) => new VarShort(Convert.ToUInt16(value, CultureInfo.InvariantCulture));


        public static bool operator !=(VarShort a, VarShort b) => !a.Equals(b);
        public static bool operator ==(VarShort a, VarShort b) => a.Equals(b);

        public bool Equals(VarShort other) => other._value.Equals(_value);
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((VarShort) obj);
        }
        public override int GetHashCode() => _value.GetHashCode();
    }
}