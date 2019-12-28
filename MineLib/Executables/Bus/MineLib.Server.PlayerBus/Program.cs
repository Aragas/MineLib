using Aragas.QServer.Core;
using Aragas.QServer.Core.Packets.PlayerHandler;

using LiteDB;

using MineLib.Core;
using MineLib.Server.Core;
using MineLib.Server.Core.Packets.PlayerHandler;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Threading.Tasks;

namespace MineLib.Server.PlayerBus
{
    /// <summary>
    /// Handles Login/Play state packets
    /// ---
    /// MineLib abstracts actions like Block placement/destructions,
    /// Player damaging, Chest access, Chat IO, etc,
    /// so multiple Protocols with different implementations
    /// can do those actions without problems.
    /// ---
    /// PlayerHandler uses multiple Protocols, so we can support
    /// multiple client versions. This way tho, we will break
    /// Mod support, in case when between 1.7.10 and 1.12 major changes
    /// were made. In such cases, ban specific versions.
    /// ---
    /// In the future, I hope to make Java Forge API mod support.
    /// With this architecture, we can assign each mod it's own process,
    /// so maybe it won't be slow as fuck and will actually work
    /// ---
    /// The Java Forge API will interchange data via a ModAPIBus, that will
    /// be implemented for both EntityBus and WorldBus.
    /// This will be another slowdown, but at least this way will give us another
    /// possibility with C#<-->Java interaction - Serialization.
    /// So, basically, the first idea with Java Forge API was to host a C# converter,
    /// that will take the jar files and do shit via JNI or similar,
    /// but now we can actually host the Java processes and because of
    /// the class serialization via network, they won't even know that
    /// they are communicating with another language.
    /// We could use protobuf serialization
    /// ---
    /// I think we should either abstract Java Forge API or abstract all Minecraft
    /// classes that the API uses. I would prefer first ofc.
    /// So we will basically replace the Forge API with out own imlementation
    /// instead of providing Forge API our own Minecraft implementation.
    /// But I think that the Forge API gives the modder the ability to
    /// use Minecraft classes, so it won't work then.
    /// It does provide Minecraft classes. It seems that the best solution will be
    /// to provide Forge API implementation and Minecraft...
    /// http://www.wuppy29.com/minecraft/modding-tutorials/forge-modding-1-8/
    /// We can implemet first such basic functionality like registering the mod
    /// and calling the initialization stages (new recepie suport etc).
    /// The best approach to start implementing the Forge APi is to follow
    /// the simple tutorial files. I will need tho a Java developer
    /// that will help me write the Java proccess, which will serialize
    /// all the classes that will be needed to interact with ModAPIBus
    /// ---
    /// </summary>
    internal sealed class Program : MineLibProgram
    {
        public static async Task Main(string[] args)
        {
            BsonMapper.Global.RegisterType
            (
                serialize: vector3 => new BsonDocument(new Dictionary<string, BsonValue>
                {
                    { nameof(Vector3.X), vector3.X  },
                    { nameof(Vector3.Y), vector3.Y  },
                    { nameof(Vector3.Z), vector3.Z  }
                }),
                deserialize: bson => new Vector3(
                    (float) bson.AsDocument[nameof(Vector3.X)].AsDouble,
                    (float) bson.AsDocument[nameof(Vector3.Y)].AsDouble,
                    (float) bson.AsDocument[nameof(Vector3.Z)].AsDouble)
            );
            BsonMapper.Global.RegisterType
            (
                serialize: look => new BsonDocument(new Dictionary<string, BsonValue>
                {
                    { nameof(Look.Pitch), look.Pitch  },
                    { nameof(Look.Yaw), look.Yaw  },
                }),
                deserialize: bson => new Look(
                    (float) bson.AsDocument[nameof(Look.Pitch)].AsDouble,
                    (float) bson.AsDocument[nameof(Look.Yaw)].AsDouble)
            );

            await Main<Program>(args).ConfigureAwait(false);
        }

        public List<ProxyConnectionHandler> ProxyConnectionHandlers { get; } = new List<ProxyConnectionHandler>();

        public override async Task RunAsync()
        {
            await base.RunAsync().ConfigureAwait(false);

            Console.WriteLine($"MineLib.Server.PlayerBus");

            var protocol5 = ProxyConnectionHandler.GetProxyConnectionHandler(5);
            protocol5.Start();
            //var protocol340 = ProxyConnectionHandler.GetProxyConnectionHandler(340);
            //protocol340.Start();
            ProxyConnectionHandlers.Add(protocol5);
            //ProxyConnectionHandlers.Add(protocol340);

            InternalBus.ProxyBus.MessageReceived += (this, ProxyBus_MessageReceived);
            InternalBus.PlayerBus.MessageReceived += (this, PlayerBus_MessageReceived);

            Console.ReadLine();
            await StopAsync().ConfigureAwait(false);
        }

        public override async Task StopAsync()
        {
            await base.StopAsync().ConfigureAwait(false);

            InternalBus.ProxyBus.MessageReceived -= ProxyBus_MessageReceived;
            InternalBus.PlayerBus.MessageReceived -= PlayerBus_MessageReceived;
            foreach (var connectionHandler in ProxyConnectionHandlers)
                connectionHandler.Stop();
        }

        private void ProxyBus_MessageReceived(object? sender, MBusMessageReceivedEventArgs args)
        {
            InternalBus.HandleRequest<AvailableSocketRequestPacket, AvailableSocketResponsePacket>(InternalBus.ProxyBus, args, request =>
            {
                var proxyConnectionHandler = ProxyConnectionHandler.GetProxyConnectionHandler(request.ProtocolVersion);
                // if ProxyBus can't handle any new players, return null in Endpoint
                //return null;

                var host = Dns.GetHostEntry(Dns.GetHostName());
                var ip = host.AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork);

                return new AvailableSocketResponsePacket()
                {
                    Endpoint = new IPEndPoint(ip, proxyConnectionHandler.Port)
                };
            });
        }
        private void PlayerBus_MessageReceived(object? sender, MBusMessageReceivedEventArgs args)
        {
            InternalBus.HandleRequest<GetPlayerDataRequestPacket, GetPlayerDataResponsePacket>(InternalBus.PlayerBus, args, request =>
            {
                if (string.IsNullOrEmpty(request.Username))
                    return new GetPlayerDataResponsePacket() { Player = null };

                using var db = new LiteDatabase("Players.db");
                var players = db.GetCollection<Player>("players");
                if (!(players.FindOne(p => p.Username==  request.Username) is Player player))
                {
                    players.Insert(player = new Player
                    {
                        Username = request.Username,
                        Uuid = Guid.NewGuid()
                    });
                }

                return new GetPlayerDataResponsePacket() { Player = player };
            });
            InternalBus.HandleRequest<UpdatePlayerDataRequestPacket, UpdatePlayerDataResponsePacket>(InternalBus.PlayerBus, args, request =>
            {
                return new UpdatePlayerDataResponsePacket()
                {
                    ErrorEnum = 0
                };
                /*
                if (string.IsNullOrEmpty(request.Player.Username))
                    return new UpdatePlayerDataResponsePacket()
                    {
                        ErrorEnum = 1
                    };

                using var db = new LiteDatabase(@"Players.db");
                var players = db.GetCollection<Player>("players");
                var player = new Player(request.Nickname)
                {
                    Position = request.Position,
                    Look = request.Look
                };
                players.Upsert(player);
                if (players.Exists(p => p.Nickname == player.Nickname))
                    players.Upsert(player);
                else
                    players.Insert(player);

                // Index document using a document property
                players.EnsureIndex(x => x.Nickname);
                return new UpdatePlayerDataResponsePacket()
                {
                    ErrorEnum = 0
                };
                */
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                InternalBus.ProxyBus.Dispose();
                InternalBus.PlayerBus.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}