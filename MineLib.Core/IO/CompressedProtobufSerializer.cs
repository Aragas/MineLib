﻿using System;

using Aragas.Network.Data;
using Aragas.Network.IO;

using Ionic.Zlib;

namespace MineLib.Core.IO
{
    public class CompressedProtobufSerializer : ProtobufSerializer
    {
        public int CompressionThreshold { get; set; } = 256;

        public override Span<byte> GetData()
        {
            if (CompressionThreshold == -1)
                return base.GetData();

            // N | Packet Length | Length of Data Length + compressed length of (Packet ID + Data)
            // N | Data Length   | Length of uncompressed (Packet ID + Data) or 0
            // C | Data          | zlib compressed packet data (see the sections below)

            Span<byte> packetData = _buffer.Length > 256 ? ZlibStream.CompressBuffer(_buffer.ToArray()) : _buffer.ToArray();
            Span<byte> dataLength = new VarInt(_buffer.Length > 256 ? (int) _buffer.Length : 0).Encode();
            Span<byte> packetLength = new VarInt(dataLength.Length + packetData.Length).Encode();

            Span<byte> data = new byte[packetLength.Length + dataLength.Length + packetData.Length];

            packetLength.CopyTo(data.Slice(0, packetLength.Length));
            dataLength.CopyTo(data.Slice(packetLength.Length, dataLength.Length));
            packetData.CopyTo(data.Slice(packetLength.Length + dataLength.Length, packetData.Length));

            _buffer = null;

            return data.ToArray();
        }
    }
}