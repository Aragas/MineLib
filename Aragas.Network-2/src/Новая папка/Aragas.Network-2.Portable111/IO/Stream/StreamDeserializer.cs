using System.IO;

namespace Aragas.Network.IO
{
    public abstract class StreamDeserializer : PacketDeserializer
    {
        public static TDeserializer Create<TDeserializer>(byte[] data) where TDeserializer : StreamDeserializer, new()
        {
            var deserializer = new TDeserializer();
            deserializer.Initialize(data);
            return deserializer;
        }

        public static TDeserializer Create<TDeserializer>(Stream stream) where TDeserializer : StreamDeserializer, new()
        {
            var deserializer = new TDeserializer();
            deserializer.Initialize(stream);
            return deserializer;
        }

        public Stream Stream { get; internal set; }

        protected StreamDeserializer() { }
        protected StreamDeserializer(byte[] data) : base(data) { }
        protected StreamDeserializer(Stream stream) => Initialize(stream);

        protected abstract void Initialize(Stream stream);
        protected sealed override void Initialize(byte[] data) => Initialize(new MemoryStream(data));

        public override int BytesLeft() => (int) (Stream.Length - Stream.Position);

        public override void Dispose()
        {
            Stream.Dispose();
        }
    }
}