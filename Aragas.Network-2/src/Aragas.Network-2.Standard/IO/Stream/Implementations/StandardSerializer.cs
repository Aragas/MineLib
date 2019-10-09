using System;
using System.Text;

namespace Aragas.Network.IO
{
    public class StandardSerializer : StreamSerializer
    {
        private Encoding Encoding { get; } = Encoding.UTF8;
        protected byte[] _buffer;

        public override Span<byte> GetData() => _buffer;


        #region Write

        // -- Anything
        public override void Write<T>(T value = default, bool writeDefaultLength = true)
        {
            var type = typeof(T);

            if (Nullable.GetUnderlyingType(type) != null)
            {
                type = Nullable.GetUnderlyingType(type);
                if (value == null)
                {
                    WriteBoolean(false);
                    return;
                }
                else
                    WriteBoolean(true);
            }

            if (type == typeof(string))
                WriteString((string)(object)value);

            else if (type == typeof(bool))
                WriteBoolean((bool)(object)value);

            else if (type == typeof(sbyte))
                WriteSByte((sbyte)(object)value);
            else if (type == typeof(byte))
                WriteUByte((byte)(object)value);

            else if (type == typeof(short))
                WriteShort((short)(object)value);
            else if (type == typeof(ushort))
                WriteUShort((ushort)(object)value);

            else if (type == typeof(int))
                WriteInt((int)(object)value);
            else if (type == typeof(uint))
                WriteUInt((uint)(object)value);

            else if (type == typeof(long))
                WriteLong((long)(object)value);
            else if (type == typeof(ulong))
                WriteULong((ulong)(object)value);

            else if (type == typeof(float))
                WriteFloat((float)(object)value);

            else if (type == typeof(double))
                WriteDouble((double)(object)value);


            else if (ExtendWriteContains(type))
                ExtendWriteExecute(this, value);


            else if (type == typeof(string[]))
                WriteStringArray((string[])(object)value, writeDefaultLength);
            else if (type == typeof(int[]))
                WriteIntArray((int[])(object)value, writeDefaultLength);
            else if (type == typeof(byte[]))
                WriteByteArray((byte[])(object)value, writeDefaultLength);
        }

        // -- String
        protected void WriteString(string value, int length = 0)
        {
            byte[] lengthBytes;
            byte[] final;

            if (length == 0)
                length = value?.Length ?? 0;

            lengthBytes = BitConverter.GetBytes(length);
            final = new byte[length + lengthBytes.Length];

            Buffer.BlockCopy(lengthBytes, 0, final, 0, lengthBytes.Length);
            if (value != null)
                Buffer.BlockCopy(Encoding.GetBytes(value), 0, final, lengthBytes.Length, length);

            ToBuffer(final);
        }

        // -- Boolean
        protected void WriteBoolean(bool value) { Write(Convert.ToByte(value)); }

        // -- SByte & Byte
        protected void WriteSByte(sbyte value) { Write(unchecked((byte)value)); }
        protected void WriteUByte(byte value) { ToBuffer(new[] { value }); }

        // -- Short & UShort
        protected void WriteShort(short value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }
        protected void WriteUShort(ushort value)
        {
            ToBuffer(new[]
            {
                (byte) ((value & 0xFF00) >> 8),
                (byte) ((value & 0xFF))
            });
        }

        // -- Int & UInt
        protected void WriteInt(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }
        protected void WriteUInt(uint value)
        {
            ToBuffer(new[]
            {
                (byte) ((value & 0xFF000000) >> 24),
                (byte) ((value & 0xFF0000) >> 16),
                (byte) ((value & 0xFF00) >> 8),
                (byte) ((value & 0xFF))
            });
        }

        // -- Long & ULong
        protected void WriteLong(long value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }
        protected void WriteULong(ulong value)
        {
            ToBuffer(new[]
            {
                (byte) ((value & 0xFF00000000000000) >> 56),
                (byte) ((value & 0xFF000000000000) >> 48),
                (byte) ((value & 0xFF0000000000) >> 40),
                (byte) ((value & 0xFF00000000) >> 32),
                (byte) ((value & 0xFF000000) >> 24),
                (byte) ((value & 0xFF0000) >> 16),
                (byte) ((value & 0xFF00) >> 8),
                (byte) ((value & 0xFF))
            });
        }

        // -- Float
        protected void WriteFloat(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }

        // -- Double
        protected void WriteDouble(double value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }

        // -- StringArray
        protected void WriteStringArray(string[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(value.Length);

            for (var i = 0; i < value.Length; i++)
                Write(value[i]);
        }

        // -- IntArray
        protected void WriteIntArray(int[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(value.Length);

            for (var i = 0; i < value.Length; i++)
                Write(value[i]);
        }

        // -- ByteArray
        protected void WriteByteArray(byte[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(value.Length);

            ToBuffer(value);
        }


        private void ToBuffer(byte[] value)
        {
            if (_buffer != null)
            {
                Array.Resize(ref _buffer, _buffer.Length + value.Length);
                Array.Copy(value, 0, _buffer, _buffer.Length - value.Length, value.Length);
            }
            else
                _buffer = value;
        }

        #endregion Write


        public override void Dispose()
        {
            _buffer = null;
        }
    }
}