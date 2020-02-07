using Newtonsoft.Json;

using System;
using System.Text;

namespace Aragas.QServer.NetworkBus.Messages
{
    public abstract class JsonMessage : IMessage
    {
        public abstract string Name { get; }

        public ReadOnlySpan<byte> GetData() => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        public void SetData(in ReadOnlySpan<byte> data) => JsonConvert.PopulateObject(Encoding.UTF8.GetString(data), this);
    }
    public abstract class JsonEnumerableMessage : IEnumerableMessage
    {
        [JsonProperty(nameof(IsLastMessage))]
        private bool _isLastMessage = default!;
        [JsonIgnore]
        public bool IsLastMessage => _isLastMessage;

        public abstract string Name { get; }

        protected JsonEnumerableMessage(bool isLast) { _isLastMessage = isLast; }

        public ReadOnlySpan<byte> GetData() => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        public void SetData(in ReadOnlySpan<byte> data) => JsonConvert.PopulateObject(Encoding.UTF8.GetString(data), this);
    }
}