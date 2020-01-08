using Ionic.Zlib;

using Newtonsoft.Json;

using System;
using System.Text;

namespace Aragas.QServer.Core.NetworkBus.Messages
{
    public abstract class BaseJsonCompressedMessage
    {
        internal class ByteArrayConverter : JsonConverter
        {
            private const int Treshold = 256;

            public override bool CanConvert(Type objectType) => objectType == typeof(byte[]);

            public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
            {
                if (value is byte[] data)
                    writer.WriteValue(Convert.ToBase64String(data.Length > Treshold ? ZlibStream.CompressBuffer(data) : data));
                else
                    writer.WriteNull();
            }

            public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
            {
                if (reader.Value is string base64 && Convert.FromBase64String(base64) is byte[] data)
                    return data.Length > Treshold ? ZlibStream.UncompressBuffer(data) : data;
                else
                    return null;
            }
        }

        protected static JsonSerializerSettings DefaultJsonSerializer => new JsonSerializerSettings()
        {
            Converters = { new ByteArrayConverter() },
            Formatting = Formatting.None,
        };
    }
    public abstract class JsonCompressedMessage : BaseJsonCompressedMessage, IMessage
    {
        public abstract string Name { get; }

        public ReadOnlySpan<byte> GetData() => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this, DefaultJsonSerializer));
        public void SetData(in ReadOnlySpan<byte> data) => JsonConvert.PopulateObject(Encoding.UTF8.GetString(data), this, DefaultJsonSerializer);
    }
    public abstract class JsonCompressedEnumerableMessage : BaseJsonCompressedMessage, IEnumerableMessage
    {
        [JsonProperty(nameof(IsLastMessage))]
        private bool _isLastMessage = default!;
        [JsonIgnore]
        public bool IsLastMessage => _isLastMessage;

        public abstract string Name { get; }

        protected JsonCompressedEnumerableMessage(bool isLast) { _isLastMessage = isLast; }

        public ReadOnlySpan<byte> GetData() => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this, DefaultJsonSerializer));
        public void SetData(in ReadOnlySpan<byte> data) => JsonConvert.PopulateObject(Encoding.UTF8.GetString(data), this, DefaultJsonSerializer);
    }
}