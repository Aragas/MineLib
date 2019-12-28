using Aragas.Network.Data;

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Aragas.Network.IO
{
    /// <summary>
    /// Data reader that uses variants for length decoding.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public class ProtobufDeserializer : StreamDeserializer
    {
        private Encoding Encoding { get; } = Encoding.UTF8;

        public ProtobufDeserializer() { }
        public ProtobufDeserializer(in Span<byte> data) : base(in data) { }
        public ProtobufDeserializer(Stream stream) : base(stream) { }

        protected override void Initialize(Stream stream)
        {
            Stream = stream;

            var dataLength = Read<VarInt>();
            var data = ReadByteArray(dataLength);
            Stream = new MemoryStream(data);
        }

        #region Read

        // -- Anything
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override T Read<T>(T value = default, int length = 0)
        {
            var type = typeof(T);

            if (Nullable.GetUnderlyingType(type) != null)
            {
                type = Nullable.GetUnderlyingType(type);
                if (!Read<bool>())
                    return value;
            }

            if (length > 0)
            {
                if (type == typeof(string))
                    return Unsafe.As<string, T>(ref ReadString(ref Unsafe.As<T, string>(ref value), length));

                if (type == typeof(string[]))
                    return Unsafe.As<string[], T>(ref ReadStringArray(ref Unsafe.As<T, string[]>(ref value), length));
                if (type == typeof(VarShort[]))
                    return Unsafe.As<VarShort[], T>(ref ReadVarShortArray(ref Unsafe.As<T, VarShort[]>(ref value), length));
                if (type == typeof(VarZShort[]))
                    return Unsafe.As<VarZShort[], T>(ref ReadVarZShortArray(ref Unsafe.As<T, VarZShort[]>(ref value), length));
                if (type == typeof(VarInt[]))
                    return Unsafe.As<VarInt[], T>(ref ReadVarIntArray(ref Unsafe.As<T, VarInt[]>(ref value), length));
                if (type == typeof(VarZInt[]))
                    return Unsafe.As<VarZInt[], T>(ref ReadVarZIntArray(ref Unsafe.As<T, VarZInt[]>(ref value), length));
                if (type == typeof(VarLong[]))
                    return Unsafe.As<VarLong[], T>(ref ReadVarLongArray(ref Unsafe.As<T, VarLong[]>(ref value), length));
                if (type == typeof(VarZLong[]))
                    return Unsafe.As<VarZLong[], T>(ref ReadVarZLongArray(ref Unsafe.As<T, VarZLong[]>(ref value), length));
                if (type == typeof(int[]))
                    return Unsafe.As<int[], T>(ref ReadIntArray(ref Unsafe.As<T, int[]>(ref value), length));
                if (type == typeof(byte[]))
                    return Unsafe.As<byte[], T>(ref ReadByteArray(ref Unsafe.As<T, byte[]>(ref value), length));


                if (ExtendReadTryExecute(this, length, out T val0))
                    return val0;


                throw new NotSupportedException($"Type {type} not found in extend methods.");
            }


            if (type == typeof(string))
                return Unsafe.As<string, T>(ref ReadString(ref Unsafe.As<T, string>(ref value), length));

            if (type == typeof(VarShort))
                return Unsafe.As<VarShort, T>(ref ReadVarShort(ref Unsafe.As<T, VarShort>(ref value)));
            if (type == typeof(VarZShort))
                return Unsafe.As<VarZShort, T>(ref ReadVarZShort(ref Unsafe.As<T, VarZShort>(ref value)));
            if (type == typeof(VarInt))
                return Unsafe.As<VarInt, T>(ref ReadVarInt(ref Unsafe.As<T, VarInt>(ref value)));
            if (type == typeof(VarZInt))
                return Unsafe.As<VarZInt, T>(ref ReadVarZInt(ref Unsafe.As<T, VarZInt>(ref value)));
            if (type == typeof(VarLong))
                return Unsafe.As<VarLong, T>(ref ReadVarLong(ref Unsafe.As<T, VarLong>(ref value)));
            if (type == typeof(VarZLong))
                return Unsafe.As<VarZLong, T>(ref ReadVarZLong(ref Unsafe.As<T, VarZLong>(ref value)));


            if (type == typeof(bool))
                return Unsafe.As<bool, T>(ref ReadBoolean(ref Unsafe.As<T, bool>(ref value)));

            if (type == typeof(sbyte))
                return Unsafe.As<sbyte, T>(ref ReadSByte(ref Unsafe.As<T, sbyte>(ref value)));
            if (type == typeof(byte))
                return Unsafe.As<byte, T>(ref ReadByte(ref Unsafe.As<T, byte>(ref value)));

            if (type == typeof(short))
                return Unsafe.As<short, T>(ref ReadShort(ref Unsafe.As<T, short>(ref value)));
            if (type == typeof(ushort))
                return Unsafe.As<ushort, T>(ref ReadUShort(ref Unsafe.As<T, ushort>(ref value)));

            if (type == typeof(int))
                return Unsafe.As<int, T>(ref ReadInt(ref Unsafe.As<T, int>(ref value)));
            if (type == typeof(uint))
                return Unsafe.As<uint, T>(ref ReadUInt(ref Unsafe.As<T, uint>(ref value)));

            if (type == typeof(long))
                return Unsafe.As<long, T>(ref ReadLong(ref Unsafe.As<T, long>(ref value)));
            if (type == typeof(ulong))
                return Unsafe.As<ulong, T>(ref ReadULong(ref Unsafe.As<T, ulong>(ref value)));

            if (type == typeof(float))
                return Unsafe.As<float, T>(ref ReadFloat(ref Unsafe.As<T, float>(ref value)));

            if (type == typeof(double))
                return Unsafe.As<double, T>(ref ReadDouble(ref Unsafe.As<T, double>(ref value)));


            if (ExtendReadTryExecute(this, length, out T val1))
                return val1;


            if (type == typeof(string[]))
                return Unsafe.As<string[], T>(ref ReadStringArray(ref Unsafe.As<T, string[]>(ref value)));
            if (type == typeof(VarShort[]))
                return Unsafe.As<VarShort[], T>(ref ReadVarShortArray(ref Unsafe.As<T, VarShort[]>(ref value)));
            if (type == typeof(VarZShort[]))
                return Unsafe.As<VarZShort[], T>(ref ReadVarZShortArray(ref Unsafe.As<T, VarZShort[]>(ref value)));
            if (type == typeof(VarInt[]))
                return Unsafe.As<VarInt[], T>(ref ReadVarIntArray(ref Unsafe.As<T, VarInt[]>(ref value)));
            if (type == typeof(VarZInt[]))
                return Unsafe.As<VarZInt[], T>(ref ReadVarZIntArray(ref Unsafe.As<T, VarZInt[]>(ref value)));
            if (type == typeof(VarLong[]))
                return Unsafe.As<VarLong[], T>(ref ReadVarLongArray(ref Unsafe.As<T, VarLong[]>(ref value)));
            if (type == typeof(VarZLong[]))
                return Unsafe.As<VarZLong[], T>(ref ReadVarZLongArray(ref Unsafe.As<T, VarZLong[]>(ref value)));
            if (type == typeof(int[]))
                return Unsafe.As<int[], T>(ref ReadIntArray(ref Unsafe.As<T, int[]>(ref value)));
            if (type == typeof(byte[]))
                return Unsafe.As<byte[], T>(ref ReadByteArray(ref Unsafe.As<T, byte[]>(ref value)));


            throw new NotSupportedException($"Type {type} not found in extend methods.");
        }

        // -- String
        private ref string ReadString(ref string value, int length = 0)
        {
            if (length == 0)
                length = Read<VarInt>();

            var stringBytes = ReadByteArray(length);
            if (stringBytes.Length != 0)
                value = Encoding.GetString(stringBytes);

            return ref value;
        }

        // -- Variants
        protected ref VarShort ReadVarShort(ref VarShort value)
        {
            value = VarShort.Decode(Stream);
            return ref value;
        }
        protected ref VarZShort ReadVarZShort(ref VarZShort value)
        {
            value = VarZShort.Decode(Stream);
            return ref value;
        }

        protected ref VarInt ReadVarInt(ref VarInt value)
        {
            value = VarInt.Decode(Stream);
            return ref value;
        }
        protected ref VarZInt ReadVarZInt(ref VarZInt value)
        {
            value = VarZInt.Decode(Stream);
            return ref value;
        }

        protected ref VarLong ReadVarLong(ref VarLong value)
        {
            value = VarLong.Decode(Stream);
            return ref value;
        }
        protected ref VarZLong ReadVarZLong(ref VarZLong value)
        {
            value = VarZLong.Decode(Stream);
            return ref value;
        }

        // -- Boolean
        protected ref bool ReadBoolean(ref bool value)
        {
            value = Convert.ToBoolean(ReadByte());
            return ref value;
        }

        // -- SByte & Byte
        protected ref sbyte ReadSByte(ref sbyte value)
        {
            value = unchecked((sbyte) ReadByte());
            return ref value;
        }
        protected ref byte ReadByte(ref byte value)
        {
            value = ReadByte();
            return ref value;
        }
        protected byte ReadByte() => (byte) Stream.ReadByte();

        // -- Short & UShort
        protected ref short ReadShort(ref short value)
        {
            Span<byte> bytes = ReadByteArray(2);
            bytes.Reverse();

            value = BitConverter.ToInt16(bytes);
            return ref value;
        }
        protected ref ushort ReadUShort(ref ushort value)
        {
            value = (ushort) ((ReadByte() << 8) | ReadByte());
            return ref value;
        }

        // -- Int & UInt
        protected ref int ReadInt(ref int value)
        {
            Span<byte> bytes = ReadByteArray(4);
            bytes.Reverse();

            value = BitConverter.ToInt32(bytes);
            return ref value;
        }
        protected ref uint ReadUInt(ref uint value)
        {
            value = (uint) (
                (ReadByte() << 24) |
                (ReadByte() << 16) |
                (ReadByte() << 8) |
                (ReadByte()));
            return ref value;
        }

        // -- Long & ULong
        protected ref long ReadLong(ref long value)
        {
            Span<byte> bytes = ReadByteArray(8);
            bytes.Reverse();

            value = BitConverter.ToInt64(bytes);
            return ref value;
        }
        protected ref ulong ReadULong(ref ulong value)
        {
            value = ((ulong) ReadByte() << 56) |
                    ((ulong) ReadByte() << 48) |
                    ((ulong) ReadByte() << 40) |
                    ((ulong) ReadByte() << 32) |
                    ((ulong) ReadByte() << 24) |
                    ((ulong) ReadByte() << 16) |
                    ((ulong) ReadByte() << 8) |
                     (ulong) ReadByte();
            return ref value;
        }

        // -- Floats
        protected ref float ReadFloat(ref float value)
        {
            Span<byte> bytes = ReadByteArray(4);
            bytes.Reverse();

            value = BitConverter.ToSingle(bytes);
            return ref value;
        }

        // -- Doubles
        protected ref double ReadDouble(ref double value)
        {
            Span<byte> bytes = ReadByteArray(8);
            bytes.Reverse();

            value = BitConverter.ToDouble(bytes);
            return ref value;
        }

        // -- StringArray
        protected ref string[] ReadStringArray(ref string[] value)
        {
            var length = Read<VarInt>();
            return ref ReadStringArray(ref value, length);
        }
        protected ref string[] ReadStringArray(ref string[] value, int length)
        {
            value = new string[length];
            for (var i = 0; i < length; i++)
                value[i] = Read<string>();
            return ref value;
        }

        // -- Variant Array
        protected ref VarShort[] ReadVarShortArray(ref VarShort[] value)
        {
            var length = Read<VarInt>();
            return ref ReadVarShortArray(ref value, length);
        }
        protected ref VarShort[] ReadVarShortArray(ref VarShort[] value, int length)
        {
            value = new VarShort[length];
            for (var i = 0; i < length; i++)
                value[i] = Read<VarShort>();
            return ref value;
        }

        protected ref VarZShort[] ReadVarZShortArray(ref VarZShort[] value)
        {
            var length = Read<VarInt>();
            return ref ReadVarZShortArray(ref value, length);
        }
        protected ref VarZShort[] ReadVarZShortArray(ref VarZShort[] value, int length)
        {
            value = new VarZShort[length];
            for (var i = 0; i < length; i++)
                value[i] = Read<VarZShort>();
            return ref value;
        }

        protected ref VarInt[] ReadVarIntArray(ref VarInt[] value)
        {
            var length = Read<VarInt>();
            return ref ReadVarIntArray(ref value, length);
        }
        protected ref VarInt[] ReadVarIntArray(ref VarInt[] value, int length)
        {
            value = new VarInt[length];
            for (var i = 0; i < length; i++)
                value[i] = Read<VarInt>();
            return ref value;
        }

        protected ref VarZInt[] ReadVarZIntArray(ref VarZInt[] value)
        {
            var length = Read<VarInt>();
            return ref ReadVarZIntArray(ref value, length);
        }
        protected ref VarZInt[] ReadVarZIntArray(ref VarZInt[] value, int length)
        {
            value = new VarZInt[length];
            for (var i = 0; i < length; i++)
                value[i] = Read<VarZInt>();
            return ref value;
        }

        protected ref VarLong[] ReadVarLongArray(ref VarLong[] value)
        {
            var length = Read<VarInt>();
            return ref ReadVarLongArray(ref value, length);
        }
        protected ref VarLong[] ReadVarLongArray(ref VarLong[] value, int length)
        {
            value = new VarLong[length];
            for (var i = 0; i < length; i++)
                value[i] = Read<VarLong>();
            return ref value;
        }

        protected ref VarZLong[] ReadVarZLongArray(ref VarZLong[] value)
        {
            var length = Read<VarInt>();
            return ref ReadVarZLongArray(ref value, length);
        }
        protected ref VarZLong[] ReadVarZLongArray(ref VarZLong[] value, int length)
        {
            value = new VarZLong[length];
            for (var i = 0; i < length; i++)
                value[i] = Read<VarZLong>();
            return ref value;
        }

        // -- IntArray
        protected ref int[] ReadIntArray(ref int[] value)
        {
            var length = Read<VarInt>();
            return ref ReadIntArray(ref value, length);
        }
        protected ref int[] ReadIntArray(ref int[] value, int length)
        {
            value = new int[length];
            for (var i = 0; i < length; i++)
                value[i] = Read<int>();
            return ref value;
        }

        // -- ByteArray
        protected ref byte[] ReadByteArray(ref byte[] value)
        {
            var length = Read<VarInt>();
            return ref ReadByteArray(ref value, length);
        }
        protected ref byte[] ReadByteArray(ref byte[] value, int length)
        {
            value = ReadByteArray(length);
            return ref value;
        }
        protected byte[] ReadByteArray(int length)
        {
            var value = new byte[length];
            var readSoFar = 0;
            while (readSoFar < length)
            {
                var read = Stream.Read(value, readSoFar, value.Length - readSoFar);
                readSoFar += read;
                if (read == 0) break;   // connection was broken
            }
            return value;
        }

        #endregion Read
    }
}