using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Aragas.Network.IO
{
    public class StandardSerializer : StreamSerializer
    {
        private Encoding Encoding { get; } = Encoding.UTF8;
        protected byte[] _buffer = Array.Empty<byte>();

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
                    Write(false);
                    return;
                }
                else
                    Write(true);
            }


            if (type == typeof(string))
                WriteString(ref Unsafe.As<T, string>(ref value));

            else if (type == typeof(bool))
                WriteBoolean(ref Unsafe.As<T, bool>(ref value));

            else if (type == typeof(sbyte))
                WriteSByte(ref Unsafe.As<T, sbyte>(ref value));
            else if (type == typeof(byte))
                WriteUByte(ref Unsafe.As<T, byte>(ref value));

            else if (type == typeof(short))
                WriteShort(ref Unsafe.As<T, short>(ref value));
            else if (type == typeof(ushort))
                WriteUShort(ref Unsafe.As<T, ushort>(ref value));

            else if (type == typeof(int))
                WriteInt(ref Unsafe.As<T, int>(ref value));
            else if (type == typeof(uint))
                WriteUInt(ref Unsafe.As<T, uint>(ref value));

            else if (type == typeof(long))
                WriteLong(ref Unsafe.As<T, long>(ref value));
            else if (type == typeof(ulong))
                WriteULong(ref Unsafe.As<T, ulong>(ref value));

            else if (type == typeof(float))
                WriteFloat(ref Unsafe.As<T, float>(ref value));

            else if (type == typeof(double))
                WriteDouble(ref Unsafe.As<T, double>(ref value));


            else if (ExtendWriteContains(type))
                ExtendWriteExecute(this, value);


            else if (type == typeof(string[]))
                WriteStringArray(ref Unsafe.As<T, string[]>(ref value), writeDefaultLength);
            else if (type == typeof(int[]))
                WriteIntArray(ref Unsafe.As<T, int[]>(ref value), writeDefaultLength);
            else if (type == typeof(byte[]))
                WriteByteArray(ref Unsafe.As<T, byte[]>(ref value), writeDefaultLength);
        }

        // -- String
        protected void WriteString(ref string value, int length = 0)
        {
            if (length == 0)
                length = value?.Length ?? 0;

            Span<byte> lengthBytes = BitConverter.GetBytes(length);
            Span<byte> final = new byte[length + lengthBytes.Length];

            lengthBytes.CopyTo(final.Slice(0, lengthBytes.Length));
            if (value != null)
                Encoding.GetBytes(value).CopyTo(final.Slice(lengthBytes.Length));

            ToBuffer(in final);
        }

        // -- Boolean
        protected void WriteBoolean(ref bool value) { Write(Convert.ToByte(value)); }

        // -- SByte & Byte
        protected void WriteSByte(ref sbyte value) { Write(unchecked((byte) value)); }
        protected void WriteUByte(ref byte value) { ToBuffer(new[] { value }); }

        // -- Short & UShort
        protected void WriteShort(ref short value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(in bytes);
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancy", "RCS1032:Remove redundant parentheses.", Justification = "Formatting")]
        protected void WriteUShort(ref ushort value)
        {
            ToBuffer(new[]
            {
                (byte) ((value & 0xFF00) >> 8),
                (byte) ((value & 0xFF))
            });
        }

        // -- Int & UInt
        protected void WriteInt(ref int value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(in bytes);
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancy", "RCS1032:Remove redundant parentheses.", Justification = "Formatting")]
        protected void WriteUInt(ref uint value)
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
        protected void WriteLong(ref long value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(in bytes);
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancy", "RCS1032:Remove redundant parentheses.", Justification = "Formatting")]
        protected void WriteULong(ref ulong value)
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
        protected void WriteFloat(ref float value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(in bytes);
        }

        // -- Double
        protected void WriteDouble(ref double value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(in bytes);
        }

        // -- StringArray
        protected void WriteStringArray(ref string[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(value.Length);

            for (var i = 0; i < value.Length; i++)
                Write(value[i]);
        }

        // -- IntArray
        protected void WriteIntArray(ref int[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(value.Length);

            for (var i = 0; i < value.Length; i++)
                Write(value[i]);
        }

        // -- ByteArray
        protected void WriteByteArray(ref byte[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(value.Length);

            ToBuffer(in value);
        }


        private void ToBuffer(in byte[] value)
        {
            if (_buffer != null)
            {
                Array.Resize(ref _buffer, _buffer.Length + value.Length);
                Array.Copy(value, 0, _buffer, _buffer.Length - value.Length, value.Length);
            }
            else
                _buffer = value;
        }
        private void ToBuffer(in Span<byte> value)
        {
            Span<byte> buffer = _buffer;
            if (_buffer != null)
            {
                Array.Resize(ref _buffer, _buffer.Length + value.Length);
                value.CopyTo(buffer.Slice(_buffer.Length - value.Length, value.Length));
            }
            else
                _buffer = value.ToArray();
        }

        #endregion Write


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _buffer = Array.Empty<byte>();
            }
        }
    }
}