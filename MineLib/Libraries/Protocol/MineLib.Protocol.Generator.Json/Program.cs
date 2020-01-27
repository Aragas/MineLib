using Newtonsoft.Json;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace MineLib.Protocol.Generator.Json
{
    public interface IPrimitive
    {

    }
    public interface IStructural { }


    public class Protocol
    {
        [JsonProperty("types"), JsonConverter(typeof(TypesConverter))]
        public List<IType> Types { get; set; }
    }
    public interface IType
    {
        public string Name { get; }
    }
    public class SimpleType : IType
    {
        public string Name { get; set; }
        public string Value { get; set; } 
    }
    public class ComplexType : IType
    {
        public string Name { get; set; }

        public List<IType> Types { get; set; }
        public Dictionary<string, string> Properties { get; set; }
    }
    public class TypesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => throw new NotImplementedException();

        private IEnumerable<SimpleType> ReadSimpleTypes(JsonReader reader)
        {
            var simpleTypes = new List<SimpleType>();
            while (
                reader.Read() && reader.TokenType == JsonToken.PropertyName && reader.Value is string name &&
                reader.Read() && reader.TokenType == JsonToken.String && reader.Value is string value)
            {
                simpleTypes.Add(new SimpleType() { Name = name, Value = value });
            }

            return simpleTypes;
        }
        private IEnumerable<ComplexType> ReadComplexTypes(JsonReader reader)
        {
            var complexTypes = new List<ComplexType>();
            while (
                /*reader.Read() &&*/ reader.TokenType == JsonToken.StartArray && reader.Value is null &&
                reader.Read() && reader.TokenType == JsonToken.String && reader.Value is string value)
            {
                var type = new ComplexType() { Name = value };
                var types = new List<IType>();
                reader.Read();
                while (
                    reader.Read() && reader.TokenType == JsonToken.PropertyName && reader.Value is string propName &&
                    reader.Read() && reader.TokenType == JsonToken.String && reader.Value is string propValue)
                {
                    types.Add(new SimpleType() { Name = propName, Value = propValue });

                    ;
                }

                complexTypes.Add(new ComplexType() { Name = value/*, Value = value*/ });
            }
            return complexTypes;
        }
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var properties1 = ReadSimpleTypes(reader);

            //var properties2 = ReadComplexTypes(reader);

            var list = new StringBuilder();
            list.Append(reader.Value ?? "null").Append(": ").Append(reader.TokenType).AppendLine();
            while (reader.Read())
                list.Append(reader.Value).Append(": ").Append(reader.TokenType).AppendLine();
            var t = list.ToString();
            return list;

            /*
            var list = new List<(object?, JsonToken)>();
            list.Add((reader.Value, reader.TokenType));
            while (reader.Read())
                list.Add((reader.Value, reader.TokenType));
            return list;
            */

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
            //var schema = JsonSchema.FromJsonAsync("  {  \"i8\":{\"enum\":[\"i8\"]},  \"u8\":{\"enum\":[\"u8\"]},  \"i16\":{\"enum\":[\"i16\"]},  \"u16\":{\"enum\":[\"u16\"]},  \"i32\":{\"enum\":[\"i32\"]},  \"u32\":{\"enum\":[\"u32\"]},  \"f32\":{\"enum\":[\"f32\"]},  \"f64\":{\"enum\":[\"f64\"]},  \"li8\":{\"enum\":[\"li8\"]},  \"lu8\":{\"enum\":[\"lu8\"]},  \"li16\":{\"enum\":[\"li16\"]},  \"lu16\":{\"enum\":[\"lu16\"]},  \"li32\":{\"enum\":[\"li32\"]},  \"lu32\":{\"enum\":[\"lu32\"]},  \"lf32\":{\"enum\":[\"lf32\"]},  \"lf64\":{\"enum\":[\"lf64\"]},  \"i64\":{\"enum\":[\"i64\"]},  \"li64\":{\"enum\":[\"li64\"]},  \"u64\":{\"enum\":[\"u64\"]},  \"lu64\":{\"enum\":[\"lu64\"]}}").Result;
            //var generator = new CSharpGenerator(schema);
            //var file = generator.GenerateFile();


            //var t0 = JsonConvert.DeserializeObject<DataPaths>(s);
            var t1 = JsonConvert.DeserializeObject<Protocol>(j);
            ;
        }
    }
}
