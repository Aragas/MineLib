using System;
using System.IO;
using System.Text;

namespace Aragas.Network.IO
{
    /// <summary>
    /// Data reader that uses int for length decoding.
    /// </summary>
    public class StandardDeserializer : StreamDeserializer
    {
        private Encoding Encoding { get; } = Encoding.UTF8;

        public StandardDeserializer() { }
        public StandardDeserializer(in Span<byte> data) : base(in data) { }
        public StandardDeserializer(Stream stream) : base(stream) { }

        protected override void Initialize(Stream stream) { }

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
                    return default!; // struct can't be null
            }

            if (length > 0)
            {
                if (type == typeof (string))
                    return (T) (object) ReadString(length);

                if (type == typeof (string[]))
                    return (T) (object) ReadStringArray(length);
                if (type == typeof (int[]))
                    return (T) (object) ReadIntArray(length);
                if (type == typeof (byte[]))
                    return (T) (object) ReadByteArray(length);


                if (ExtendReadTryExecute(this, length, out val))
                    return val;


                throw new NotSupportedException($"Type {type} not found in extend methods.");
            }


            if (type == typeof (string))
                return (T) (object) ReadString();

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
            if (type == typeof (int[]))
                return (T) (object) ReadIntArray();
            if (type == typeof (byte[]))
                return (T) (object) ReadByteArray();


            throw new NotSupportedException($"Type {type} not found in extend methods.");
        }

        // -- String
        protected string ReadString(int length = 0)
        {
            if (length == 0)
                length = ReadInt();

            var stringBytes = ReadByteArray(length);
            if (stringBytes.Length == 0)
                return string.Empty;

            return Encoding.GetString(stringBytes, 0, stringBytes.Length);
        }

        // -- Boolean
        protected bool ReadBoolean() { return Convert.ToBoolean(ReadByte()); }

        // -- SByte & Byte
        protected sbyte ReadSByte() { return unchecked((sbyte) ReadByte()); }
        protected byte ReadByte() { return (byte) Stream.ReadByte(); }

        // -- Short & UShort
        protected short ReadShort()
        {
            var bytes = ReadByteArray(2);
            Array.Reverse(bytes);

            return BitConverter.ToInt16(bytes, 0);
        }
        protected ushort ReadUShort()
        {
            return (ushort) ((ReadByte() << 8) | ReadByte());
        }

        // -- Int & UInt
        protected int ReadInt()
        {
            var bytes = ReadByteArray(4);
            Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
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
            var bytes = ReadByteArray(8);
            Array.Reverse(bytes);

            return BitConverter.ToInt64(bytes, 0);
        }
        protected ulong ReadULong()
        {
            return unchecked(
                   ((ulong) ReadByte() << 56) |
                   ((ulong) ReadByte() << 48) |
                   ((ulong) ReadByte() << 40) |
                   ((ulong) ReadByte() << 32) |
                   ((ulong) ReadByte() << 24) |
                   ((ulong) ReadByte() << 16) |
                   ((ulong) ReadByte() << 8) |
                    (ulong) ReadByte());
        }

        // -- Floats
        protected float ReadFloat()
        {
            var bytes = ReadByteArray(4);
            Array.Reverse(bytes);

            return BitConverter.ToSingle(bytes, 0);
        }

        // -- Doubles
        protected double ReadDouble()
        {
            var bytes = ReadByteArray(8);
            Array.Reverse(bytes);

            return BitConverter.ToDouble(bytes, 0);
        }

        // -- StringArray
        protected string[] ReadStringArray()
        {
            var length = ReadInt();
            return ReadStringArray(length);
        }
        protected string[] ReadStringArray(int length)
        {
            var myStrings = new string[length];

            for (var i = 0; i < length; i++)
                myStrings[i] = ReadString();

            return myStrings;
        }

        // -- IntArray
        protected int[] ReadIntArray()
        {
            var length = ReadInt();
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
            var length = ReadInt();
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
    }
}