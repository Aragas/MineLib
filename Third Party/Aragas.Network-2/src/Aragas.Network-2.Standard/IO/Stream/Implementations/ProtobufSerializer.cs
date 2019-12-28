using Aragas.Network.Data;

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Aragas.Network.IO
{
    /// <summary>
    /// Stream that uses variant for length encoding.
    /// </summary>
    public class ProtobufSerializer : StreamSerializer
    {
        private Encoding Encoding { get; } = Encoding.UTF8;
        protected MemoryStream _buffer = new MemoryStream();

        public override Span<byte> GetData()
        {
            Span<byte> packetID_packetData = _buffer.ToArray();
            Span<byte> length = new VarInt(packetID_packetData.Length).Encode();
            Span<byte> data = new byte[length.Length + packetID_packetData.Length];

            length.CopyTo(data.Slice(0, length.Length));
            packetID_packetData.CopyTo(data.Slice(length.Length, packetID_packetData.Length));

            _buffer.Dispose();
            _buffer = new MemoryStream();

            return data;
        }


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

            if (type == typeof (string))
                WriteString(ref Unsafe.As<T, string>(ref value));

            else if (type == typeof (VarShort))
                WriteVarShort(ref Unsafe.As<T, VarShort>(ref value));
            else if (type == typeof (VarZShort))
                WriteVarZShort(ref Unsafe.As<T, VarZShort>(ref value));
            else if (type == typeof (VarInt))
                WriteVarInt(ref Unsafe.As<T, VarInt>(ref value));
            else if (type == typeof (VarZInt))
                WriteVarZInt(ref Unsafe.As<T, VarZInt>(ref value));
            else if (type == typeof (VarLong))
                WriteVarLong(ref Unsafe.As<T, VarLong>(ref value));
            else if (type == typeof (VarZLong))
                WriteVarZLong(ref Unsafe.As<T, VarZLong>(ref value));


            else if (type == typeof (bool))
                WriteBoolean(ref Unsafe.As<T, bool>(ref value));

            else if (type == typeof (sbyte))
                WriteSByte(ref Unsafe.As<T, sbyte>(ref value));
            else if (type == typeof (byte))
                WriteUByte(ref Unsafe.As<T, byte>(ref value));

            else if (type == typeof (short))
                WriteShort(ref Unsafe.As<T, short>(ref value));
            else if (type == typeof (ushort))
                WriteUShort(ref Unsafe.As<T, ushort>(ref value));

            else if (type == typeof (int))
                WriteInt(ref Unsafe.As<T, int>(ref value));
            else if (type == typeof (uint))
                WriteUInt(ref Unsafe.As<T, uint>(ref value));

            else if (type == typeof (long))
                WriteLong(ref Unsafe.As<T, long>(ref value));
            else if (type == typeof (ulong))
                WriteULong(ref Unsafe.As<T, ulong>(ref value));

            else if (type == typeof (float))
                WriteFloat(ref Unsafe.As<T, float>(ref value));

            else if (type == typeof (double))
                WriteDouble(ref Unsafe.As<T, double>(ref value));


            else if (ExtendWriteContains(type))
                ExtendWriteExecute(this, value, writeDefaultLength);


            else if (type == typeof (string[]))
                WriteStringArray(ref Unsafe.As<T, string[]>(ref value), writeDefaultLength);
            else if (type == typeof (int[]))
                WriteIntArray(ref Unsafe.As<T, int[]>(ref value), writeDefaultLength);
            else if (type == typeof (byte[]))
                WriteByteArray(ref Unsafe.As<T, byte[]>(ref value), writeDefaultLength);
            else if (type == typeof (VarShort[]))
                WriteVarShortArray(ref Unsafe.As<T, VarShort[]>(ref value), writeDefaultLength);
            else if (type == typeof (VarZShort[]))
                WriteVarZShortArray(ref Unsafe.As<T, VarZShort[]>(ref value), writeDefaultLength);
            else if (type == typeof (VarInt[]))
                WriteVarIntArray(ref Unsafe.As<T, VarInt[]>(ref value), writeDefaultLength);
            else if (type == typeof (VarZInt[]))
                WriteVarZIntArray(ref Unsafe.As<T, VarZInt[]>(ref value), writeDefaultLength);
            else if (type == typeof (VarLong[]))
                WriteVarLongArray(ref Unsafe.As<T, VarLong[]>(ref value), writeDefaultLength);
            else if (type == typeof (VarZLong[]))
                WriteVarZLongArray(ref Unsafe.As<T, VarZLong[]>(ref value), writeDefaultLength);
        }

        // -- String
        private void WriteString(ref string value, int length = 0)
        {
            if (length == 0)
                length = value?.Length ?? 0;

            Span<byte> lengthBytes = new VarInt(length).Encode();
            Span<byte> final = new byte[length + lengthBytes.Length];

            lengthBytes.CopyTo(final.Slice(0, lengthBytes.Length));
            if(value != null)
                Encoding.GetBytes(value).CopyTo(final.Slice(lengthBytes.Length));

            ToBuffer(in final);
        }

        // -- Variants
        private void WriteVarShort(ref VarShort value) => ToBuffer(value.Encode());
        private void WriteVarZShort(ref VarZShort value) => ToBuffer(value.Encode());

        private void WriteVarInt(ref VarInt value) => ToBuffer(value.Encode());
        private void WriteVarZInt(ref VarZInt value) => ToBuffer(value.Encode());

        private void WriteVarLong(ref VarLong value) => ToBuffer(value.Encode());
        private void WriteVarZLong(ref VarZLong value) => ToBuffer(value.Encode());

        // -- Boolean
        private void WriteBoolean(ref bool value) => Write(Convert.ToByte(value));

        // -- SByte & Byte
        private void WriteSByte(ref sbyte value) => Write(unchecked((byte)value));
        private void WriteUByte(ref byte value) => ToBuffer(new[] { value });

        // -- Short & UShort
        private void WriteShort(ref short value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            //bytes.Reverse();

            ToBuffer(in bytes);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancy", "RCS1032:Remove redundant parentheses.", Justification = "Formatting")]
        private void WriteUShort(ref ushort value)
        {
            ToBuffer(new[]
            {
                (byte) ((value & 0xFF00) >> 8),
                (byte) ((value & 0xFF))
            });
        }

        // -- Int & UInt
        private void WriteInt(ref int value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            //bytes.Reverse();

            ToBuffer(in bytes);
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancy", "RCS1032:Remove redundant parentheses.", Justification = "Formatting")]
        private void WriteUInt(ref uint value)
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
        private void WriteLong(ref long value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            //bytes.Reverse();

            ToBuffer(in bytes);
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancy", "RCS1032:Remove redundant parentheses.", Justification = "Formatting")]
        private void WriteULong(ref ulong value)
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
        private void WriteFloat(ref float value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            //bytes.Reverse();

            ToBuffer(in bytes);
        }

        // -- Double
        private void WriteDouble(ref double value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            //bytes.Reverse();

            ToBuffer(in bytes);
        }

        // -- StringArray
        private void WriteStringArray(ref string[] value, bool writeDefaultLength)
        {
            if(writeDefaultLength)
                Write(new VarInt(value.Length));

            for (var i = 0; i < value.Length; i++)
                WriteString(ref value[i]);
        }

        // -- Variable Array
        private void WriteVarShortArray(ref VarShort[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(new VarInt(value.Length));

            for (var i = 0; i < value.Length; i++)
                WriteVarShort(ref value[i]);
        }
        private void WriteVarZShortArray(ref VarZShort[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(new VarInt(value.Length));

            for (var i = 0; i < value.Length; i++)
                WriteVarZShort(ref value[i]);
        }

        private void WriteVarIntArray(ref VarInt[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(new VarInt(value.Length));

            for (var i = 0; i < value.Length; i++)
                WriteVarInt(ref value[i]);
        }
        private void WriteVarZIntArray(ref VarZInt[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(new VarInt(value.Length));

            for (var i = 0; i < value.Length; i++)
                WriteVarZInt(ref value[i]);
        }

        private void WriteVarLongArray(ref VarLong[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(new VarInt(value.Length));

            for (var i = 0; i < value.Length; i++)
                WriteVarLong(ref value[i]);
        }
        private void WriteVarZLongArray(ref VarZLong[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(new VarInt(value.Length));

            for (var i = 0; i < value.Length; i++)
                WriteVarZLong(ref value[i]);
        }

        // -- IntArray
        private void WriteIntArray(ref int[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(new VarInt(value.Length));

            for (var i = 0; i < value.Length; i++)
                WriteInt(ref value[i]);
        }

        // -- ByteArray
        private void WriteByteArray(ref byte[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(new VarInt(value.Length));

            ToBuffer(in value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Simplification", "RCS1180:Inline lazy initialization.", Justification = "Not intended here")]
        private void ToBuffer(in byte[] value)
        {
            if (_buffer == null)
                _buffer = new MemoryStream();

            _buffer.Write(value);
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Simplification", "RCS1180:Inline lazy initialization.", Justification = "Not intended here")]
        private void ToBuffer(in Span<byte> value)
        {
            if (_buffer == null)
                _buffer = new MemoryStream();

            _buffer.Write(value);

            /*
            if (_buffer != null)
            {
                Array.Resize(ref _buffer, _buffer.Length + value.Length);
                Array.Copy(value, 0, _buffer, _buffer.Length - value.Length, value.Length);
            }
            else
                _buffer = value;
            */
        }
        /*
        private void ToBuffer(in ReadOnlySpan<byte> value)
        {
            if (_buffer != null)
                _buffer.Write(value);
            else
            {
                _buffer = new MemoryStream();
                _buffer.Write(value);
            }
        }
        */

        #endregion Write


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _buffer?.Dispose();
                _buffer = default!; // intended
            }
        }
    }
}