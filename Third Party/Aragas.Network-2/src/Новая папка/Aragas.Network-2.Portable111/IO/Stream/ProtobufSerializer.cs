using System;
using System.Text;

using Aragas.Network.Data;

using Ionic.Zlib;

namespace Aragas.Network.IO
{
    public class CompressedProtobufSerializer : ProtobufSerializer
    {
        public int CompressionThreshold { get; set; } = 256;

        public override byte[] GetData()
        {
            if (CompressionThreshold != -1)
                return base.GetData();

            // N | Packet Length | Length of Data Length + compressed length of (Packet ID + Data)
            // N | Data Length   | Length of uncompressed (Packet ID + Data) or 0
            // C | Data          | zlib compressed packet data (see the sections below)

            Span<byte> packetData = _buffer.Length > 256 ? ZlibStream.CompressBuffer(_buffer) : _buffer;
            Span<byte> dataLength = new VarInt(_buffer.Length > 256 ? _buffer.Length : 0).Encode();
            Span<byte> packetLength = new VarInt(dataLength.Length + packetData.Length).Encode();

            Span<byte> data = new byte[packetLength.Length + dataLength.Length + packetData.Length];

            packetLength.CopyTo(data.Slice(0, packetLength.Length));
            dataLength.CopyTo(data.Slice(packetLength.Length, dataLength.Length));
            packetData.CopyTo(data.Slice(packetLength.Length + dataLength.Length, packetData.Length));

            _buffer = null;

            return data.ToArray();
        }
    }

    /// <summary>
    /// Stream that uses variant for length encoding.
    /// </summary>
    public class ProtobufSerializer : StreamSerializer
    {
        private Encoding Encoding { get; } = Encoding.UTF8;
        protected byte[] _buffer;

        public override byte[] GetData()
        {
            Span<byte> packetID_packetData = _buffer;
            Span<byte> length = new VarInt(packetID_packetData.Length).Encode();
            Span<byte> data = new byte[length.Length + packetID_packetData.Length];

            length.CopyTo(data.Slice(0, length.Length));
            packetID_packetData.CopyTo(data.Slice(length.Length, packetID_packetData.Length));

            _buffer = null;

            return data.ToArray();
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
                    WriteBoolean(false);
                    return;
                }
                else
                    WriteBoolean(true);
            }

            if (type == typeof (string))
                WriteString((string) (object) value);

            else if (type == typeof (VarShort))
                WriteVarShort((VarShort) (object) value);
            else if (type == typeof (VarZShort))
                WriteVarZShort((VarZShort) (object) value);
            else if (type == typeof (VarInt))
                WriteVarInt((VarInt) (object) value);
            else if (type == typeof (VarZInt))
                WriteVarZInt((VarZInt) (object) value);
            else if (type == typeof (VarLong))
                WriteVarLong((VarLong) (object) value);
            else if (type == typeof (VarZLong[]))
                WriteVarZLong((VarZLong) (object) value);


            else if (type == typeof (bool))
                WriteBoolean((bool) (object) value);

            else if (type == typeof (sbyte))
                WriteSByte((sbyte) (object) value);
            else if (type == typeof (byte))
                WriteUByte((byte) (object) value);

            else if (type == typeof (short))
                WriteShort((short) (object) value);
            else if (type == typeof (ushort))
                WriteUShort((ushort) (object) value);

            else if (type == typeof (int))
                WriteInt((int) (object) value);
            else if (type == typeof (uint))
                WriteUInt((uint) (object) value);

            else if (type == typeof (long))
                WriteLong((long) (object) value);
            else if (type == typeof (ulong))
                WriteULong((ulong) (object) value);

            else if (type == typeof (float))
                WriteFloat((float) (object) value);

            else if (type == typeof (double))
                WriteDouble((double) (object) value);


            else if (ExtendWriteContains(type))
                ExtendWriteExecute(this, value, writeDefaultLength);


            else if (type == typeof (string[]))
                WriteStringArray((string[]) (object) value, writeDefaultLength);
            else if (type == typeof (int[]))
                WriteIntArray((int[]) (object) value, writeDefaultLength);
            else if (type == typeof (byte[]))
                WriteByteArray((byte[]) (object) value, writeDefaultLength);
            else if (type == typeof (VarShort[]))
                WriteVarShortArray((VarShort[]) (object) value, writeDefaultLength);
            else if (type == typeof (VarZShort[]))
                WriteVarZShortArray((VarZShort[]) (object) value, writeDefaultLength);
            else if (type == typeof (VarInt[]))
                WriteVarIntArray((VarInt[]) (object) value, writeDefaultLength);
            else if (type == typeof (VarZInt[]))
                WriteVarZIntArray((VarZInt[]) (object) value, writeDefaultLength);
            else if (type == typeof (VarLong[]))
                WriteVarLongArray((VarLong[]) (object) value, writeDefaultLength);
            else if (type == typeof (VarZLong[]))
                WriteVarZLongArray((VarZLong[]) (object) value, writeDefaultLength);
        }

        // -- String
        private void WriteString(string value, int length = 0)
        {
            if (length == 0)
                length = value?.Length ?? 0;

            var lengthBytes = new VarInt(length).Encode();
            var final = new byte[length + lengthBytes.Length];
            

            Buffer.BlockCopy(lengthBytes, 0, final, 0, lengthBytes.Length);
            if(value != null)
                Buffer.BlockCopy(Encoding.GetBytes(value), 0, final, lengthBytes.Length, length);

            ToBuffer(final);
        }

        // -- Variants
        private void WriteVarShort(VarShort value) { ToBuffer(value.Encode()); }
        private void WriteVarZShort(VarZShort value) { ToBuffer(value.Encode()); }

        private void WriteVarInt(VarInt value) { ToBuffer(value.Encode()); }
        private void WriteVarZInt(VarZInt value) { ToBuffer(value.Encode()); }

        private void WriteVarLong(VarLong value) { ToBuffer(value.Encode()); }
        private void WriteVarZLong(VarZLong value) { ToBuffer(value.Encode()); }

        // -- Boolean
        private void WriteBoolean(bool value) { Write(Convert.ToByte(value)); }

        // -- SByte & Byte
        private void WriteSByte(sbyte value) { Write(unchecked((byte) value)); }
        private void WriteUByte(byte value) { ToBuffer(new[] { value }); }

        // -- Short & UShort
        private void WriteShort(short value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }
        private void WriteUShort(ushort value)
        {
            ToBuffer(new[]
            {
                (byte) ((value & 0xFF00) >> 8),
                (byte) ((value & 0xFF))
            });
        }

        // -- Int & UInt
        private void WriteInt(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }
        private void WriteUInt(uint value)
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
        private void WriteLong(long value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }
        private void WriteULong(ulong value)
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
        private void WriteFloat(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }

        // -- Double
        private void WriteDouble(double value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            ToBuffer(bytes);
        }

        // -- StringArray
        private void WriteStringArray(string[] value, bool writeDefaultLength)
        {
            if(writeDefaultLength)
                Write(new VarInt(value.Length));

            for (var i = 0; i < value.Length; i++)
                Write(value[i]);
        }

        // -- Variable Array
        private void WriteVarShortArray(VarShort[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(new VarInt(value.Length));

            for (var i = 0; i < value.Length; i++)
                Write(value[i]);
        }
        private void WriteVarZShortArray(VarZShort[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(new VarInt(value.Length));

            for (var i = 0; i < value.Length; i++)
                Write(value[i]);
        }

        private void WriteVarIntArray(VarInt[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(new VarInt(value.Length));

            for (var i = 0; i < value.Length; i++)
                Write(value[i]);
        }
        private void WriteVarZIntArray(VarZInt[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(new VarInt(value.Length));

            for (var i = 0; i < value.Length; i++)
                Write(value[i]);
        }

        private void WriteVarLongArray(VarLong[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(new VarInt(value.Length));

            for (var i = 0; i < value.Length; i++)
                Write(value[i]);
        }
        private void WriteVarZLongArray(VarZLong[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(new VarInt(value.Length));

            for (var i = 0; i < value.Length; i++)
                Write(value[i]);
        }

        // -- IntArray
        private void WriteIntArray(int[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(new VarInt(value.Length));

            for (var i = 0; i < value.Length; i++)
                Write(value[i]);
        }

        // -- ByteArray
        private void WriteByteArray(byte[] value, bool writeDefaultLength)
        {
            if (writeDefaultLength)
                Write(new VarInt(value.Length));

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