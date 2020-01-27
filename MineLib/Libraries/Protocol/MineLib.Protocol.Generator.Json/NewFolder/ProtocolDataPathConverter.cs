using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace MineLib.Protocol.Generator.Json
{
    public class ProtocolDataPathConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => throw new NotImplementedException();

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var list = new List<ProtocolDataPath>();
            while (TryReadProtocol(reader, serializer, out var info))
                list.Add(info!);
            return list;
        }

        private bool TryReadProtocol(JsonReader reader, JsonSerializer serializer, out ProtocolDataPath? info)
        {
            if (reader.Read() && reader.TokenType == JsonToken.PropertyName && reader.Value is string version && reader.Read())
            {
                info = serializer.Deserialize<ProtocolDataPath>(reader);
                info!.Name = version;
                return true;
            }
            else
            {
                info = null;
                return false;
            }
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
