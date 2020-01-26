using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace MineLib.Protocol.Generator.Json
{
    public class Protocols
    {
        [JsonProperty("pc"), JsonConverter(typeof(ProtocolInfoConverter))]
        public List<ProtocolInfo> Vanilla { get; set; }

        [JsonProperty("pe"), JsonConverter(typeof(ProtocolInfoConverter))]
        public List<ProtocolInfo> Bedrock { get; set; }
    }
    public class ProtocolInfo
    {
        public string Name { get; set; }


        [JsonProperty("blocks")]
        public string Blocks { get; set; }

        [JsonProperty("biomes")]
        public string Biomes { get; set; }

        [JsonProperty("effects")]
        public string Effects { get; set; }

        [JsonProperty("enchantments")]
        public string Enchantments { get; set; }

        [JsonProperty("items")]
        public string Items { get; set; }

        [JsonProperty("recipes")]
        public string Recipes { get; set; }

        [JsonProperty("instruments")]
        public string Instruments { get; set; }

        [JsonProperty("materials")]
        public string Materials { get; set; }

        [JsonProperty("entities")]
        public string Entities { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("windows")]
        public string Windows { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("protocolComments")]
        public string ProtocolComments { get; set; }
    }
    public class ProtocolInfoConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => throw new NotImplementedException();

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var list = new List<ProtocolInfo>();
            while (TryReadProtocol(reader, serializer, out var info))
                list.Add(info!);
            return list;
        }

        private bool TryReadProtocol(JsonReader reader, JsonSerializer serializer, out ProtocolInfo? info)
        {
            if(reader.Read() && reader.TokenType == JsonToken.PropertyName && reader.Value is string version && reader.Read())
            {
                info = serializer.Deserialize<ProtocolInfo>(reader);
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

    public class Protocol
    {
        public class Types
        {

        }

        [JsonProperty("types"), JsonConverter(typeof(TypesConverter))]
        public Types Type { get; set; }
    }
    public class TypesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => throw new NotImplementedException();

        private Dictionary<string, string> ReadSimpleProperties(JsonReader reader)
        {
            var properties = new Dictionary<string, string>();
            while (
                reader.Read() && reader.TokenType == JsonToken.PropertyName && reader.Value is string name &&
                reader.Read() && reader.TokenType == JsonToken.String && reader.Value is string value)
                properties.Add(name, value);
            return properties;
        }
        private Dictionary<string, string> ReadSimpleProperties1(JsonReader reader)
        {
            var properties = new Dictionary<string, string>();
            while (
                reader.Read() && reader.TokenType == JsonToken.StartArray && reader.Value is string name &&
                reader.Read() && reader.TokenType == JsonToken.String && reader.Value is string value)
            {
                properties.Add(name, value);
            }
            return properties;
        }
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var properties = ReadSimpleProperties(reader);

            var list = new List<(object?, JsonToken)>();
            list.Add((reader.Value, reader.TokenType));
            while (reader.Read())
                list.Add((reader.Value, reader.TokenType));
            return list;

            /*
            reader.Read();
            var list = new List<ProtocolInfo>();
            while (TryReadProtocol(reader, serializer, out var info))
                list.Add(info!);
            return list;
            */
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) => throw new NotImplementedException();
    }

    public static class Program
    {
        static string s = "{  \"pc\": {    \"0.30c\": {      \"blocks\": \"pc/0.30c\",      \"protocol\": \"pc/0.30c\",      \"version\": \"pc/0.30c\"    },    \"1.7\": {      \"blocks\": \"pc/1.7\",      \"biomes\": \"pc/1.7\",      \"enchantments\": \"pc/1.7\",      \"effects\": \"pc/1.7\",      \"items\": \"pc/1.7\",      \"recipes\": \"pc/1.8\",      \"instruments\": \"pc/1.7\",      \"materials\": \"pc/1.7\",      \"entities\": \"pc/1.7\",      \"protocol\": \"pc/1.7\",      \"windows\": \"pc/1.7\",      \"version\": \"pc/1.7\",      \"language\":\"pc/1.7\"    },    \"1.8\": {      \"blocks\": \"pc/1.8\",      \"biomes\": \"pc/1.8\",      \"enchantments\": \"pc/1.8\",      \"effects\": \"pc/1.8\",      \"items\": \"pc/1.8\",      \"recipes\": \"pc/1.8\",      \"instruments\": \"pc/1.8\",      \"materials\": \"pc/1.8\",      \"entities\": \"pc/1.8\",      \"protocol\": \"pc/1.8\",      \"windows\": \"pc/1.8\",      \"version\": \"pc/1.8\",      \"language\":\"pc/1.8\"    },    \"15w40b\": {      \"blocks\": \"pc/1.9\",      \"biomes\": \"pc/1.9\",      \"effects\": \"pc/1.9\",      \"enchantments\": \"pc/1.9\",      \"items\": \"pc/1.9\",      \"recipes\": \"pc/1.9\",      \"instruments\": \"pc/1.9\",      \"materials\": \"pc/1.9\",      \"entities\": \"pc/1.9\",      \"protocol\": \"pc/15w40b\",      \"windows\": \"pc/1.9\",      \"version\": \"pc/15w40b\",      \"language\":\"pc/1.9\"    },    \"1.9\": {      \"blocks\": \"pc/1.9\",      \"biomes\": \"pc/1.9\",      \"effects\": \"pc/1.9\",      \"enchantments\": \"pc/1.9\",      \"items\": \"pc/1.9\",      \"recipes\": \"pc/1.9\",      \"instruments\": \"pc/1.9\",      \"materials\": \"pc/1.9\",      \"entities\": \"pc/1.9\",      \"protocol\": \"pc/1.9\",      \"windows\": \"pc/1.9\",      \"version\": \"pc/1.9\",      \"language\":\"pc/1.9\"    },    \"1.9.1-pre2\": {      \"blocks\": \"pc/1.9\",      \"biomes\": \"pc/1.9\",      \"effects\": \"pc/1.9\",      \"enchantments\": \"pc/1.9\",      \"items\": \"pc/1.9\",      \"recipes\": \"pc/1.9\",      \"instruments\": \"pc/1.9\",      \"materials\": \"pc/1.9\",      \"entities\": \"pc/1.9\",      \"protocol\": \"pc/1.9.1-pre2\",      \"windows\": \"pc/1.9\",      \"version\": \"pc/1.9.1-pre2\",      \"language\":\"pc/1.9\"    },    \"1.9.2\": {      \"blocks\": \"pc/1.9\",      \"biomes\": \"pc/1.9\",      \"effects\": \"pc/1.9\",      \"enchantments\": \"pc/1.9\",      \"items\": \"pc/1.9\",      \"recipes\": \"pc/1.9\",      \"instruments\": \"pc/1.9\",      \"materials\": \"pc/1.9\",      \"entities\": \"pc/1.9\",      \"protocol\": \"pc/1.9.2\",      \"protocolComments\": \"pc/1.9.2\",      \"windows\": \"pc/1.9\",      \"version\": \"pc/1.9.2\",      \"language\":\"pc/1.9\"    },    \"1.9.4\": {      \"blocks\": \"pc/1.9\",      \"biomes\": \"pc/1.9\",      \"effects\": \"pc/1.9\",      \"enchantments\": \"pc/1.9\",      \"items\": \"pc/1.9\",      \"recipes\": \"pc/1.9\",      \"instruments\": \"pc/1.9\",      \"materials\": \"pc/1.9\",      \"entities\": \"pc/1.9\",      \"protocol\": \"pc/1.9.4\",      \"windows\": \"pc/1.9\",      \"version\": \"pc/1.9.4\",      \"language\":\"pc/1.9\"    },    \"16w20a\": {      \"blocks\": \"pc/1.9\",      \"biomes\": \"pc/1.9\",      \"effects\": \"pc/1.9\",      \"enchantments\": \"pc/1.9\",      \"items\": \"pc/1.9\",      \"recipes\": \"pc/1.9\",      \"instruments\": \"pc/1.9\",      \"materials\": \"pc/1.9\",      \"entities\": \"pc/1.9\",      \"protocol\": \"pc/16w20a\",      \"windows\": \"pc/1.9\",      \"version\": \"pc/16w20a\",      \"language\":\"pc/1.9\"    },    \"1.10-pre1\": {      \"blocks\": \"pc/1.9\",      \"biomes\": \"pc/1.9\",      \"effects\": \"pc/1.9\",      \"enchantments\": \"pc/1.9\",      \"items\": \"pc/1.9\",      \"recipes\": \"pc/1.9\",      \"instruments\": \"pc/1.9\",      \"materials\": \"pc/1.9\",      \"entities\": \"pc/1.9\",      \"protocol\": \"pc/1.10-pre1\",      \"windows\": \"pc/1.9\",      \"version\": \"pc/1.10-pre1\",      \"language\":\"pc/1.9\"    },    \"1.10\": {      \"blocks\": \"pc/1.10\",      \"biomes\": \"pc/1.10\",      \"effects\": \"pc/1.10\",      \"enchantments\": \"pc/1.10\",      \"items\": \"pc/1.10\",      \"recipes\": \"pc/1.10\",      \"instruments\": \"pc/1.10\",      \"materials\": \"pc/1.10\",      \"entities\": \"pc/1.10\",      \"protocol\": \"pc/1.10\",      \"windows\": \"pc/1.10\",      \"version\": \"pc/1.10\",      \"language\":\"pc/1.10\"    },    \"1.10.1\": {      \"blocks\": \"pc/1.10\",      \"biomes\": \"pc/1.10\",      \"effects\": \"pc/1.10\",      \"enchantments\": \"pc/1.10\",      \"items\": \"pc/1.10\",      \"recipes\": \"pc/1.10\",      \"instruments\": \"pc/1.10\",      \"materials\": \"pc/1.10\",      \"entities\": \"pc/1.10\",      \"protocol\": \"pc/1.10\",      \"windows\": \"pc/1.10\",      \"version\": \"pc/1.10.1\",      \"language\":\"pc/1.10\"    },    \"1.10.2\": {      \"blocks\": \"pc/1.10\",      \"biomes\": \"pc/1.10\",      \"effects\": \"pc/1.10\",      \"enchantments\": \"pc/1.10\",      \"items\": \"pc/1.10\",      \"recipes\": \"pc/1.10\",      \"instruments\": \"pc/1.10\",      \"materials\": \"pc/1.10\",      \"entities\": \"pc/1.10\",      \"protocol\": \"pc/1.10\",      \"windows\": \"pc/1.10\",      \"version\": \"pc/1.10.2\",      \"language\":\"pc/1.10\"    },    \"16w35a\": {      \"blocks\": \"pc/1.10\",      \"biomes\": \"pc/1.10\",      \"effects\": \"pc/1.10\",      \"enchantments\": \"pc/1.10\",      \"items\": \"pc/1.10\",      \"recipes\": \"pc/1.10\",      \"instruments\": \"pc/1.10\",      \"materials\": \"pc/1.10\",      \"entities\": \"pc/1.10\",      \"protocol\": \"pc/16w35a\",      \"windows\": \"pc/1.10\",      \"version\": \"pc/16w35a\",      \"language\":\"pc/1.10\"    },    \"1.11\": {      \"blocks\": \"pc/1.11\",      \"biomes\": \"pc/1.11\",      \"effects\": \"pc/1.11\",      \"enchantments\": \"pc/1.11\",      \"items\": \"pc/1.11\",      \"recipes\": \"pc/1.11\",      \"instruments\": \"pc/1.11\",      \"materials\": \"pc/1.11\",      \"entities\": \"pc/1.11\",      \"protocol\": \"pc/1.11\",      \"protocolComments\": \"pc/1.11.2\",      \"windows\": \"pc/1.11\",      \"version\": \"pc/1.11\",      \"language\":\"pc/1.11\"    },    \"1.11.2\": {      \"blocks\": \"pc/1.11\",      \"biomes\": \"pc/1.11\",      \"effects\": \"pc/1.11\",      \"enchantments\": \"pc/1.11\",      \"items\": \"pc/1.11\",      \"recipes\": \"pc/1.11\",      \"instruments\": \"pc/1.11\",      \"materials\": \"pc/1.11\",      \"entities\": \"pc/1.11\",      \"protocol\": \"pc/1.11\",      \"protocolComments\": \"pc/1.11.2\",      \"windows\": \"pc/1.11\",      \"version\": \"pc/1.11.2\",      \"language\":\"pc/1.11\"    },    \"17w15a\": {      \"blocks\": \"pc/1.11\",      \"biomes\": \"pc/1.11\",      \"effects\": \"pc/1.11\",      \"enchantments\": \"pc/1.11\",      \"items\": \"pc/1.11\",      \"recipes\": \"pc/1.11\",      \"instruments\": \"pc/1.11\",      \"materials\": \"pc/1.11\",      \"entities\": \"pc/1.11\",      \"protocol\": \"pc/17w15a\",      \"windows\": \"pc/1.11\",      \"version\": \"pc/17w15a\",      \"language\":\"pc/1.11\"    },    \"17w18b\": {      \"blocks\": \"pc/1.11\",      \"biomes\": \"pc/1.11\",      \"effects\": \"pc/1.11\",      \"enchantments\": \"pc/1.11\",      \"items\": \"pc/1.11\",      \"recipes\": \"pc/1.11\",      \"instruments\": \"pc/1.11\",      \"materials\": \"pc/1.11\",      \"entities\": \"pc/1.11\",      \"protocol\": \"pc/17w18b\",      \"windows\": \"pc/1.11\",      \"version\": \"pc/17w18b\",      \"language\":\"pc/1.11\"    },    \"1.12-pre4\": {      \"blocks\": \"pc/1.11\",      \"biomes\": \"pc/1.11\",      \"effects\": \"pc/1.11\",      \"enchantments\": \"pc/1.11\",      \"items\": \"pc/1.11\",      \"recipes\": \"pc/1.11\",      \"instruments\": \"pc/1.11\",      \"materials\": \"pc/1.11\",      \"entities\": \"pc/1.11\",      \"protocol\": \"pc/1.12-pre4\",      \"windows\": \"pc/1.11\",      \"version\": \"pc/1.12-pre4\",      \"language\":\"pc/1.11\"    },    \"1.12\": {      \"blocks\": \"pc/1.12\",      \"biomes\": \"pc/1.12\",      \"effects\": \"pc/1.12\",      \"enchantments\": \"pc/1.12\",      \"items\": \"pc/1.12\",      \"recipes\": \"pc/1.12\",      \"instruments\": \"pc/1.12\",      \"materials\": \"pc/1.12\",      \"entities\": \"pc/1.12\",      \"protocol\": \"pc/1.12\",      \"windows\": \"pc/1.12\",      \"version\": \"pc/1.12\",      \"language\":\"pc/1.12\"    },    \"1.12.1\": {      \"blocks\": \"pc/1.12\",      \"biomes\": \"pc/1.12\",      \"effects\": \"pc/1.12\",      \"enchantments\": \"pc/1.12\",      \"items\": \"pc/1.12\",      \"recipes\": \"pc/1.12\",      \"instruments\": \"pc/1.12\",      \"materials\": \"pc/1.12\",      \"entities\": \"pc/1.12\",      \"protocol\": \"pc/1.12.1\",      \"windows\": \"pc/1.12\",      \"version\": \"pc/1.12.1\",      \"language\":\"pc/1.12\"    },    \"1.12.2\": {      \"blocks\": \"pc/1.12\",      \"biomes\": \"pc/1.12\",      \"effects\": \"pc/1.12\",      \"enchantments\": \"pc/1.12\",      \"items\": \"pc/1.12\",      \"recipes\": \"pc/1.12\",      \"instruments\": \"pc/1.12\",      \"materials\": \"pc/1.12\",      \"entities\": \"pc/1.12\",      \"protocol\": \"pc/1.12.2\",      \"windows\": \"pc/1.12\",      \"version\": \"pc/1.12.2\",      \"language\":\"pc/1.12\"    },    \"17w50a\": {      \"blocks\": \"pc/1.12\",      \"biomes\": \"pc/1.12\",      \"effects\": \"pc/1.12\",      \"enchantments\": \"pc/1.12\",      \"items\": \"pc/1.12\",      \"recipes\": \"pc/1.12\",      \"instruments\": \"pc/1.12\",      \"materials\": \"pc/1.12\",      \"entities\": \"pc/1.12\",      \"protocol\": \"pc/17w50a\",      \"windows\": \"pc/1.12\",      \"version\": \"pc/17w50a\",      \"language\":\"pc/1.12\"    },    \"1.13\": {      \"blocks\": \"pc/1.13\",      \"biomes\": \"pc/1.13\",      \"effects\": \"pc/1.13\",      \"enchantments\": \"pc/1.13\",      \"items\": \"pc/1.13\",      \"recipes\": \"pc/1.13\",      \"instruments\": \"pc/1.13\",      \"materials\": \"pc/1.13\",      \"entities\": \"pc/1.13\",      \"protocol\": \"pc/1.13\",      \"windows\": \"pc/1.13\",      \"version\": \"pc/1.13\",      \"language\":\"pc/1.13\"    },    \"1.13.1\": {      \"blocks\": \"pc/1.13\",      \"biomes\": \"pc/1.13\",      \"effects\": \"pc/1.13\",      \"enchantments\": \"pc/1.13\",      \"items\": \"pc/1.13\",      \"recipes\": \"pc/1.13\",      \"instruments\": \"pc/1.13\",      \"materials\": \"pc/1.13\",      \"entities\": \"pc/1.13\",      \"protocol\": \"pc/1.13.1\",      \"windows\": \"pc/1.13\",      \"version\": \"pc/1.13.1\",      \"language\":\"pc/1.13\"    },    \"1.13.2-pre1\": {      \"blocks\": \"pc/1.13\",      \"biomes\": \"pc/1.13\",      \"effects\": \"pc/1.13\",      \"enchantments\": \"pc/1.13\",      \"items\": \"pc/1.13\",      \"recipes\": \"pc/1.13\",      \"instruments\": \"pc/1.13\",      \"materials\": \"pc/1.13\",      \"entities\": \"pc/1.13\",      \"protocol\": \"pc/1.13.2-pre1\",      \"windows\": \"pc/1.13\",      \"version\": \"pc/1.13.2-pre1\",      \"language\":\"pc/1.13\"    },    \"1.13.2-pre2\": {      \"blocks\": \"pc/1.13\",      \"biomes\": \"pc/1.13\",      \"effects\": \"pc/1.13\",      \"enchantments\": \"pc/1.13\",      \"items\": \"pc/1.13\",      \"recipes\": \"pc/1.13\",      \"instruments\": \"pc/1.13\",      \"materials\": \"pc/1.13\",      \"entities\": \"pc/1.13\",      \"protocol\": \"pc/1.13.2-pre2\",      \"windows\": \"pc/1.13\",      \"version\": \"pc/1.13.2-pre2\",      \"language\":\"pc/1.13\"    },    \"1.13.2\": {      \"blocks\": \"pc/1.13.2\",      \"biomes\": \"pc/1.13.2\",      \"effects\": \"pc/1.13.2\",      \"enchantments\": \"pc/1.13.2\",      \"items\": \"pc/1.13.2\",      \"recipes\": \"pc/1.13.2\",      \"instruments\": \"pc/1.13.2\",      \"materials\": \"pc/1.13.2\",      \"entities\": \"pc/1.13.2\",      \"protocol\": \"pc/1.13.2\",      \"windows\": \"pc/1.13.2\",      \"version\": \"pc/1.13.2\",      \"language\":\"pc/1.13.2\"    },    \"1.14\": {      \"biomes\": \"pc/1.14\",      \"items\": \"pc/1.14\",      \"recipes\": \"pc/1.14\",      \"entities\": \"pc/1.14\",      \"language\": \"pc/1.14\",      \"protocol\": \"pc/1.14\",      \"version\": \"pc/1.14\"    },    \"1.14.1\": {      \"biomes\": \"pc/1.14\",      \"items\": \"pc/1.14\",      \"recipes\": \"pc/1.14\",      \"entities\": \"pc/1.14\",      \"language\": \"pc/1.14\",      \"protocol\": \"pc/1.14.1\",      \"version\": \"pc/1.14.1\"    },    \"1.14.3\": {      \"biomes\": \"pc/1.14\",      \"items\": \"pc/1.14\",      \"recipes\": \"pc/1.14\",      \"entities\": \"pc/1.14\",      \"language\": \"pc/1.14\",      \"protocol\": \"pc/1.14.3\",      \"version\": \"pc/1.14.3\"    },    \"1.14.4\": {      \"biomes\": \"pc/1.14\",      \"items\": \"pc/1.14\",      \"recipes\": \"pc/1.14\",      \"entities\": \"pc/1.14\",      \"language\": \"pc/1.14\",      \"protocol\": \"pc/1.14.4\",      \"version\": \"pc/1.14.4\"    },    \"1.15\": {      \"protocol\": \"pc/1.15\",      \"version\": \"pc/1.15\"    },    \"1.15.1\": {      \"protocol\": \"pc/1.15.1\",      \"version\": \"pc/1.15.1\"    }  },  \"pe\":{    \"0.14\":{      \"blocks\": \"pe/0.14\",      \"biomes\": \"pc/1.8\",      \"items\": \"pe/0.14\",      \"protocol\": \"pe/0.14\",      \"version\": \"pe/0.14\"    },    \"0.15\":{      \"blocks\": \"pe/0.15\",      \"biomes\": \"pc/1.8\",      \"items\": \"pe/0.15\",      \"protocol\": \"pe/0.15\",      \"version\": \"pe/0.15\"    },    \"1.0\":{      \"blocks\": \"pe/1.0\",      \"biomes\": \"pc/1.8\",      \"items\": \"pe/1.0\",      \"version\": \"pe/1.0\"    }  }}";
        static string j = "{  \"types\": {    \"varint\": \"native\",    \"pstring\": \"native\",    \"u16\": \"native\",    \"u8\": \"native\",    \"i64\": \"native\",    \"buffer\": \"native\",    \"i32\": \"native\",    \"i8\": \"native\",    \"bool\": \"native\",    \"i16\": \"native\",    \"f32\": \"native\",    \"f64\": \"native\",    \"UUID\": \"native\",    \"option\": \"native\",    \"entityMetadataLoop\": \"native\",    \"bitfield\": \"native\",    \"container\": \"native\",    \"switch\": \"native\",    \"void\": \"native\",    \"array\": \"native\",    \"restBuffer\": \"native\",    \"nbt\": \"native\",    \"compressedNbt\": \"native\",    \"string\": [      \"pstring\",      {        \"countType\": \"varint\"      }    ],    \"slot\": [      \"container\",      [        {          \"name\": \"blockId\",          \"type\": \"i16\"        },        {          \"anon\": true,          \"type\": [            \"switch\",            {              \"compareTo\": \"blockId\",              \"fields\": {                \"-1\": \"void\"              },              \"default\": [                \"container\",                [                  {                    \"name\": \"itemCount\",                    \"type\": \"i8\"                  },                  {                    \"name\": \"itemDamage\",                    \"type\": \"i16\"                  },                  {                    \"name\": \"nbtData\",                    \"type\": \"compressedNbt\"                  }                ]              ]            }          ]        }      ]    ],    \"position_iii\": [      \"container\",      [        {          \"name\": \"x\",          \"type\": \"i32\"        },        {          \"name\": \"y\",          \"type\": \"i32\"        },        {          \"name\": \"z\",          \"type\": \"i32\"        }      ]    ],    \"position_isi\": [      \"container\",      [        {          \"name\": \"x\",          \"type\": \"i32\"        },        {          \"name\": \"y\",          \"type\": \"i16\"        },        {          \"name\": \"z\",          \"type\": \"i32\"        }      ]    ],    \"position_ibi\": [      \"container\",      [        {          \"name\": \"x\",          \"type\": \"i32\"        },        {          \"name\": \"y\",          \"type\": \"u8\"        },        {          \"name\": \"z\",          \"type\": \"i32\"        }      ]    ],    \"entityMetadataItem\": [      \"switch\",      {        \"compareTo\": \"$compareTo\",        \"fields\": {          \"0\": \"i8\",          \"1\": \"i16\",          \"2\": \"i32\",          \"3\": \"f32\",          \"4\": \"string\",          \"5\": \"slot\",          \"6\": [            \"container\",            [              {                \"name\": \"x\",                \"type\": \"i32\"              },              {                \"name\": \"y\",                \"type\": \"i32\"              },              {                \"name\": \"z\",                \"type\": \"i32\"              }            ]          ],          \"7\": [            \"container\",            [              {                \"name\": \"pitch\",                \"type\": \"f32\"              },              {                \"name\": \"yaw\",                \"type\": \"f32\"              },              {                \"name\": \"roll\",                \"type\": \"f32\"              }            ]          ]        }      }    ],    \"entityMetadata\": [      \"entityMetadataLoop\",      {        \"endVal\": 127,        \"type\": [          \"container\",          [            {              \"anon\": true,              \"type\": [                \"bitfield\",                [                  {                    \"name\": \"type\",                    \"size\": 3,                    \"signed\": false                  },                  {                    \"name\": \"key\",                    \"size\": 5,                    \"signed\": false                  }                ]              ]            },            {              \"name\": \"value\",              \"type\": [                \"entityMetadataItem\",                {                  \"compareTo\": \"type\"                }              ]            }          ]        ]      }    ]  }}";
        public static void Main(string[] args)
        {
            //var t0 = JsonConvert.DeserializeObject<Protocols>(s);
            var t1 = JsonConvert.DeserializeObject<Protocol>(j);
            ;
        }
    }
}
