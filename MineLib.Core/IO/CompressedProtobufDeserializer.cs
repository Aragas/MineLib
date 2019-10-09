﻿using System;
using System.IO;

using Aragas.Network.Data;
using Aragas.Network.IO;

using Ionic.Zlib;

namespace MineLib.Core.IO
{
    public class CompressedProtobufDeserializer : ProtobufDeserializer
    {
        public int CompressionThreshold { get; set; } = 256;

        public CompressedProtobufDeserializer() { }
        public CompressedProtobufDeserializer(in Span<byte> data) : base(in data) { }
        public CompressedProtobufDeserializer(Stream stream) : base(stream) { }

        protected override void Initialize(Stream stream)
        {
            if(CompressionThreshold == -1)
                Initialize(stream);
            Stream = stream;

            // N | Packet Length | Length of Data Length + compressed length of (Packet ID + Data)
            // N | Data Length   | Length of uncompressed (Packet ID + Data) or 0
            // C | Data          | zlib compressed packet data (see the sections below)

            var packetLength = ReadVarInt();
            var dataLength = ReadVarInt();
            var actualDataLength = packetLength - new VarInt(dataLength).Size;
            var actualData = ReadByteArray(actualDataLength);

            var data = dataLength == 0 ? actualData : ZlibStream.UncompressBuffer(actualData);
            Stream = new MemoryStream(data);
        }
    }
}