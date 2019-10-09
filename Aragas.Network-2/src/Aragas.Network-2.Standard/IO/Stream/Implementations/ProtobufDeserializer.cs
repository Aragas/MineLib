using System;
using System.IO;
using System.Text;

using Aragas.Network.Data;

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
            var dataLength = ReadVarInt();
            var data = ReadByteArray(dataLength);
            Stream = new MemoryStream(data);
        }

        #region Read

        // -- Anything
        public override T Read<T>(T value = default, int length = 0)
        {
            T val;
            var type = typeof(T);

            if (Nullable.GetUnderlyingType(type) != null)
            {
                type = Nullable.GetUnderlyingType(type);
                if (!ReadBoolean())
                    return default;
            }

            if (length > 0)
            {
                if (type == typeof (string))
                    return (T) (object) ReadString(length);

                if (type == typeof (string[]))
                    return (T) (object) ReadStringArray(length);
                if (type == typeof (VarShort[]))
                    return (T) (object) ReadVarShortArray(length);
                if (type == typeof (VarZShort[]))
                    return (T) (object) ReadVarZShortArray(length);
                if (type == typeof (VarInt[]))
                    return (T) (object) ReadVarIntArray(length);
                if (type == typeof (VarZInt[]))
                    return (T) (object) ReadVarZIntArray(length);
                if (type == typeof (VarLong[]))
                    return (T) (object) ReadVarLongArray(length);
                if (type == typeof (VarZLong[]))
                    return (T) (object) ReadVarZLongArray(length);
                if (type == typeof (int[]))
                    return (T) (object) ReadIntArray(length);
                if (type == typeof (byte[]))
                    return (T) (object) ReadByteArray(length);


                if(ExtendReadTryExecute(this, length, out val))
                    return val;


                throw new NotImplementedException($"Type {type} not found in extend methods.");
            }


            if (type == typeof (string))
                return (T) (object) ReadString();

            if (type == typeof (VarShort))
                return (T) (object) ReadVarShort();
            if (type == typeof (VarZShort))
                return (T) (object) ReadVarZShort();
            if (type == typeof (VarInt))
                return (T) (object) ReadVarInt();
            if (type == typeof (VarZInt))
                return (T) (object) ReadVarZInt();
            if (type == typeof (VarLong))
                return (T) (object) ReadVarLong();
            if (type == typeof (VarZLong))
                return (T) (object) ReadVarZLong();


            if (type == typeof (bool))
                return (T) (object) ReadBoolean();

            if (type == typeof (sbyte))
                return (T) (object) ReadSByte();
            if (type == typeof (byte))
                return (T) (object) ReadByte();

            if (type == typeof (short))
                return (T) (object) ReadShort();
            if (type == typeof (ushort))
                return (T) (object) ReadUShort();

            if (type == typeof (int))
                return (T) (object) ReadInt();
            if (type == typeof (uint))
                return (T) (object) ReadUInt();

            if (type == typeof (long))
                return (T) (object) ReadLong();
            if (type == typeof (ulong))
                return (T) (object) ReadULong();

            if (type == typeof (float))
                return (T) (object) ReadFloat();

            if (type == typeof (double))
                return (T) (object) ReadDouble();


            if (ExtendReadTryExecute(this, length, out val))
                return val;


            if (type == typeof (string[]))
                return (T) (object) ReadStringArray();
            if (type == typeof (VarShort[]))
                return (T) (object) ReadVarShortArray();
            if (type == typeof (VarZShort[]))
                return (T) (object) ReadVarZShortArray();
            if (type == typeof (VarInt[]))
                return (T) (object) ReadVarIntArray();
            if (type == typeof (VarZInt[]))
                return (T) (object) ReadVarZIntArray();
            if (type == typeof (VarLong[]))
                return (T) (object) ReadVarLongArray();
            if (type == typeof (VarZLong[]))
                return (T) (object) ReadVarZLongArray();
            if (type == typeof (int[]))
                return (T) (object) ReadIntArray();
            if (type == typeof (byte[]))
                return (T) (object) ReadByteArray();
            

            throw new NotImplementedException($"Type {type} not found in extend methods.");
        }

        // -- String
        private string ReadString(int length = 0)
        {
            if (length == 0)
                length = ReadVarInt();

            var stringBytes = ReadByteArray(length);
            if (stringBytes.Length == 0)
                return null;

            return Encoding.GetString(stringBytes);
        }

        // -- Variants
        protected VarShort ReadVarShort() => VarShort.Decode(Stream);
        protected VarZShort ReadVarZShort() => VarZShort.Decode(Stream);

        protected VarInt ReadVarInt() => VarInt.Decode(Stream);
        protected VarZInt ReadVarZInt() => VarZInt.Decode(Stream);

        protected VarLong ReadVarLong() => VarLong.Decode(Stream);
        protected VarZLong ReadVarZLong() => VarZLong.Decode(Stream);

        // -- Boolean
        protected bool ReadBoolean() => Convert.ToBoolean(ReadByte());

        // -- SByte & Byte
        protected sbyte ReadSByte() => unchecked((sbyte) ReadByte());
        protected byte ReadByte() => (byte) Stream.ReadByte();

        // -- Short & UShort
        protected short ReadShort()
        {
            Span<byte> bytes = ReadByteArray(2);
            bytes.Reverse();

            return BitConverter.ToInt16(bytes);
        }
        protected ushort ReadUShort()
        {
            return (ushort) ((ReadByte() << 8) | ReadByte());
        }

        // -- Int & UInt
        protected int ReadInt()
        {
            Span<byte> bytes = ReadByteArray(4);
            bytes.Reverse();

            return BitConverter.ToInt32(bytes);
        }
        protected uint ReadUInt()
        {
            return (uint) (
                (ReadByte() << 24) |
                (ReadByte() << 16) |
                (ReadByte() << 8) |
                (ReadByte()));
        }

        // -- Long & ULong
        protected long ReadLong()
        {
            Span<byte> bytes = ReadByteArray(8);
            bytes.Reverse();

            return BitConverter.ToInt64(bytes);
        }
        protected ulong ReadULong()
        {
            return ((ulong) ReadByte() << 56) |
                   ((ulong) ReadByte() << 48) |
                   ((ulong) ReadByte() << 40) |
                   ((ulong) ReadByte() << 32) |
                   ((ulong) ReadByte() << 24) |
                   ((ulong) ReadByte() << 16) |
                   ((ulong) ReadByte() << 8) |
                    (ulong) ReadByte();
        }

        // -- Floats
        protected float ReadFloat()
        {
            Span<byte> bytes = ReadByteArray(4);
            bytes.Reverse();

            return BitConverter.ToSingle(bytes);
        }

        // -- Doubles
        protected double ReadDouble()
        {
            Span<byte> bytes = ReadByteArray(8);
            bytes.Reverse();

            return BitConverter.ToDouble(bytes);
        }

        // -- StringArray
        protected string[] ReadStringArray()
        {
            var length = ReadVarInt();
            return ReadStringArray(length);
        }
        protected string[] ReadStringArray(int length)
        {
            var myStrings = new string[length];

            for (var i = 0; i < length; i++)
                myStrings[i] = ReadString();

            return myStrings;
        }

        // -- Variant Array
        protected VarShort[] ReadVarShortArray()
        {
            var length = ReadVarInt();
            return ReadVarShortArray(length);
        }
        protected VarShort[] ReadVarShortArray(int length)
        {
            var myInts = new VarShort[length];

            for (var i = 0; i < length; i++)
                myInts[i] = ReadVarShort();

            return myInts;
        }

        protected VarZShort[] ReadVarZShortArray()
        {
            var length = ReadVarInt();
            return ReadVarZShortArray(length);
        }
        protected VarZShort[] ReadVarZShortArray(int length)
        {
            var myInts = new VarZShort[length];

            for (var i = 0; i < length; i++)
                myInts[i] = ReadVarZShort();

            return myInts;
        }

        protected VarInt[] ReadVarIntArray()
        {
            var length = ReadVarInt();
            return ReadVarIntArray(length);
        }
        protected VarInt[] ReadVarIntArray(int length)
        {
            var myInts = new VarInt[length];

            for (var i = 0; i < length; i++)
                myInts[i] = ReadVarInt();

            return myInts;
        }

        protected VarZInt[] ReadVarZIntArray()
        {
            var length = ReadVarInt();
            return ReadVarZIntArray(length);
        }
        protected VarZInt[] ReadVarZIntArray(int length)
        {
            var myInts = new VarZInt[length];

            for (var i = 0; i < length; i++)
                myInts[i] = ReadVarZInt();

            return myInts;
        }

        protected VarLong[] ReadVarLongArray()
        {
            var length = ReadVarInt();
            return ReadVarLongArray(length);
        }
        protected VarLong[] ReadVarLongArray(int length)
        {
            var myInts = new VarLong[length];

            for (var i = 0; i < length; i++)
                myInts[i] = ReadVarLong();

            return myInts;
        }

        protected VarZLong[] ReadVarZLongArray()
        {
            var length = ReadVarInt();
            return ReadVarZLongArray(length);
        }
        protected VarZLong[] ReadVarZLongArray(int length)
        {
            var myInts = new VarZLong[length];

            for (var i = 0; i < length; i++)
                myInts[i] = ReadVarZLong();

            return myInts;
        }

        // -- IntArray
        protected int[] ReadIntArray()
        {
            var length = ReadVarInt();
            return ReadIntArray(length);
        }
        protected int[] ReadIntArray(int length)
        {
            var myInts = new int[length];

            for (var i = 0; i < length; i++)
                myInts[i] = ReadInt();

            return myInts;
        }

        // -- ByteArray
        protected byte[] ReadByteArray()
        {
            var length = ReadVarInt();
            return ReadByteArray(length);
        }
        protected byte[] ReadByteArray(int length)
        {
            if (length == 0)
                return new byte[length];

            var msg = new byte[length];
            var readSoFar = 0;
            while (readSoFar < length)
            {
                var read = Stream.Read(msg, readSoFar, msg.Length - readSoFar);
                readSoFar += read;
                if (read == 0)
                    break;   // connection was broken
            }

            return msg;
        }

        #endregion Read

        public override void Dispose()
        {
            
        }
    }
}