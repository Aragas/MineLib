using App.Metrics.Health;
using App.Metrics.Health.Checks.Sql;

using Aragas.QServer.Core;
using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.NetworkBus.Messages;

using MineLib.Server.Core;
using MineLib.Server.Core.NetworkBus.Messages;

using Npgsql;

using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Threading;
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
        public static async Task Main(string[] args) => await Main<Program>(args).ConfigureAwait(false);

        private ManualResetEvent Waiter { get; } = new ManualResetEvent(false);
        private CompositeDisposable Events { get; } = new CompositeDisposable();
        private ConcurrentDictionary<Guid, PlayerHandler.PlayerHandler> PlayerHanlders { get; } = new ConcurrentDictionary<Guid, PlayerHandler.PlayerHandler>();

        public Program() : base(healthConfigure: ConfigureHealth)
        {
            Events.Add(BaseSingleton.Instance.SubscribeAndReply<ServicesPingMessage>(_ =>
                new ServicesPongMessage() { ServiceId = ProgramGuid, ServiceType = "PlayerBus" }));

            Events.Add(BaseSingleton.Instance.SubscribeAndReply<GetExistingPlayerHandlerRequestMessage>(
                message =>
                {
                    if (PlayerHanlders.TryGetValue(message.PlayerId, out var playerHandler) && playerHandler.ProtocolVersion == message.ProtocolVersion)
                        return new GetExistingPlayerHandlerResponseMessage() { ServiceId = ProgramGuid, State = playerHandler.State!.Value };
                    else
                        return new GetExistingPlayerHandlerResponseMessage() { ServiceId = null };
                }));
            Events.Add(BaseSingleton.Instance.SubscribeAndReplyToExclusive<GetNewPlayerHandlerRequestMessage, GetNewPlayerHandlerResponseMessage>(
                message =>
                {
                    return true;
                },
                message =>
                {
                    var stuff = new PlayerHandler.PlayerHandler(message.PlayerId, message.ProtocolVersion);
                    PlayerHanlders.TryAdd(message.PlayerId, stuff);

                    return new GetNewPlayerHandlerResponseMessage() { ServiceId = ProgramGuid };
                }, ProgramGuid));
        }
        public static IHealthBuilder ConfigureHealth(IHealthBuilder builder) => builder
            .HealthChecks.AddProcessPhysicalMemoryCheck("Process Working Set Size", 100 * 1024 * 1024)
            .HealthChecks.AddProcessPrivateMemorySizeCheck("Process Private Memory Size", 100 * 1024 * 1024)
            .HealthChecks.AddSqlCheck("PosgreSQL Connection", () => new NpgsqlConnection(MineLibSingleton.PostgreSQLConnectionString), TimeSpan.FromMilliseconds(500));

        public override async Task RunAsync()
        {
            await base.RunAsync().ConfigureAwait(false);

            Waiter.WaitOne();
        }

        public override async Task StopAsync()
        {
            await base.StopAsync().ConfigureAwait(false);

            Waiter.Set();
        }

        /*
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
        }
        */

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var playerHandler in PlayerHanlders)
                    playerHandler.Value.Dispose();

                Waiter.Dispose();
                Events.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}