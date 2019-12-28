using System.Collections.Generic;
using System.Linq;

using MineLib.Core.Client;

using Newtonsoft.Json;

namespace ProtocolModern.Data
{
    public struct ServerResponse : IServerResponse
    {
        public ServerData Info { get; internal set; }

        public long Ping { get; internal set; }
        public byte[] Icon => Info.Favicon;
        public string Brand => Info.Version.Name;
        public int Protocol => Info.Version.Protocol;
        public string Description => Info.Description;
        public int Max => Info.Players.Max;
        public int Online => Info.Players.Online;
        public List<IServerPlayer> Players => Info.Players.Sample.Select(player => (IServerPlayer)player).ToList();
    }
    public struct ServerData
    {
        public struct ServerVersion
        {
            [JsonProperty("name")]
            public string Name { get; private set; }

            [JsonProperty("protocol")]
            public int Protocol { get; private set; }
        }

        public struct ServerPlayers
        {
            public struct PlayerSample : IServerPlayer
            {
                [JsonProperty("name")]
                public string Name { get; private set; }

                [JsonProperty("id")]
                public string ID { get; private set; }
            }


            [JsonProperty("max")]
            public int Max { get; private set; }

            [JsonProperty("online")]
            public int Online { get; private set; }

            [JsonProperty("sample")]
            public List<PlayerSample> Sample { get; private set; }
        }


        [JsonProperty("version")]
        public ServerVersion Version { get; private set; }

        [JsonProperty("players")]
        public ServerPlayers Players { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("favicon")]
        public byte[] Favicon { get; private set; }

        [JsonProperty("modinfo")]
        public ForgeModInfo? ModInfo { get; private set; }
    }
    public struct ForgeModInfo
    {
        public struct Mod
        {
            [JsonProperty("modid")]
            public string ModID { get; private set; }

            [JsonProperty("version")]
            public string Version { get; private set; }
        }


        [JsonProperty("type")]
        public string Type { get; private set; }

        [JsonProperty("modList")]
        public List<Mod> Mods { get; private set; }
    }
}
