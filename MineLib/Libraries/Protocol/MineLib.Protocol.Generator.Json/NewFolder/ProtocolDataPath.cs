using Newtonsoft.Json;

namespace MineLib.Protocol.Generator.Json
{
    public class ProtocolDataPath
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

        public override string ToString() => Name;
    }
}
