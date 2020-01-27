using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MineLib.Protocol.Generator.ProtoDef.Builtin.Numeric
{
    public static class Converter
    {
        public static string SimpleToNETType(this string protoDefType) => protoDefType switch
        {
            "i8" => typeof(sbyte).FullName!,
            "u8" => typeof(byte).FullName!,

            "i16" => typeof(short).FullName!,
            "u16" => typeof(ushort).FullName!,

            "i32" => typeof(int).FullName!,
            "u32" => typeof(uint).FullName!,

            "i64" => typeof(long).FullName!,
            "u64" => typeof(ulong).FullName!,


            "f32" => typeof(float).FullName!,
            "f64" => typeof(double).FullName!,

            _ => throw new NotSupportedException(),
        };

        public static string ArrayToNETType(this string protoDefType, string countType) => $"{SimpleToNETType(protoDefType)}";
    }

    //[JsonProperty("i8")]
    public class i8
    {
    }
}
