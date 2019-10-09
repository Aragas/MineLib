using System;
using System.IO;

namespace Aragas.Network.IO
{
    public abstract class StreamDeserializer : PacketDeserializer
    {
        public static TDeserializer Create<TDeserializer>(in Span<byte> data) where TDeserializer : StreamDeserializer, new()
        {
            var deserializer = new TDeserializer();
            deserializer.Initialize(in data);
            return deserializer;
        }

        public static TDeserializer Create<TDeserializer>(Stream stream) where TDeserializer : StreamDeserializer, new()
        {
            var deserializer = new TDeserializer();
            deserializer.Initialize(stream);
            return deserializer;
        }

        public Stream Stream { get; protected internal set; }

        protected StreamDeserializer() { }
        protected StreamDeserializer(in Span<byte> data) : base(in data) { }
        protected StreamDeserializer(Stream stream) => Initialize(stream);

        protected abstract void Initialize(Stream stream);
        protected sealed override void Initialize(in Span<byte> data) => Initialize(new MemoryStream(data.ToArray()));

        public override int BytesLeft() => (int) (Stream.Length - Stream.Position);

        public override void Dispose()
        {
            Stream.Dispose();
        }
    }
}