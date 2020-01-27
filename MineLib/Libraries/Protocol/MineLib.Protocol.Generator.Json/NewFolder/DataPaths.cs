using Newtonsoft.Json;
using System.Collections.Generic;

namespace MineLib.Protocol.Generator.Json
{
    public class DataPaths
    {
        [JsonProperty("pc"), JsonConverter(typeof(ProtocolDataPathConverter))]
        public List<ProtocolDataPath> Vanilla { get; set; }

        [JsonProperty("pe"), JsonConverter(typeof(ProtocolDataPathConverter))]
        public List<ProtocolDataPath> Bedrock { get; set; }
    }
}
