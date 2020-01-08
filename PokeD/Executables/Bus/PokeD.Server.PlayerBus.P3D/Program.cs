using App.Metrics.Health;

using Aragas.QServer.Core;
using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.NetworkBus.Messages;

using PokeD.Server.Core;
using PokeD.Server.Core.Data;
using PokeD.Server.Core.NetworkBus.Messages;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace PokeD.Server.PlayerBus.P3D
{
    internal sealed class Program : PokeDProgram
    {
        public static async Task Main(string[] args) => await Main<Program>(args).ConfigureAwait(false);

        private ManualResetEvent Waiter { get; } = new ManualResetEvent(false);
        private CompositeDisposable Events { get; } = new CompositeDisposable();
        private List<P3DPlayer> players = new List<P3DPlayer>();

        public Program() : base(healthConfigure: ConfigureHealth)
        {
            Events.Add(BaseSingleton.Instance.SubscribeAndReply<ServicesPingMessage>(_ =>
                new ServicesPongMessage() { ServiceId = ProgramGuid, ServiceType = "PlayerBus.P3D" }));
            Events.Add(BaseSingleton.Instance.SubscribeAndReply<GetExistingPlayerHandlerRequestMessage>(
                message =>
                {
                    return new GetExistingPlayerHandlerResponseMessage();
                }));
            Events.Add(BaseSingleton.Instance.SubscribeAndReplyToExclusive<GetNewPlayerHandlerRequestMessage, GetNewPlayerHandlerResponseMessage>(
                message => message.PlayerType == PlayerType.P3D,
                message =>
                {
                    players.Add(new P3DPlayer(message.PlayerId));

                    return new GetNewPlayerHandlerResponseMessage() { ServiceId = ProgramGuid };
                }, ProgramGuid));
        }
        public static IHealthBuilder ConfigureHealth(IHealthBuilder builder) => builder
            .HealthChecks.AddProcessPhysicalMemoryCheck("Process Working Set Size", 100 * 1024 * 1024)
            .HealthChecks.AddProcessPrivateMemorySizeCheck("Process Private Memory Size", 100 * 1024 * 1024)
            //.HealthChecks.AddSqlCheck("PosgreSQL Connection", () => new NpgsqlConnection(PostgresStatic.ConnectionString), TimeSpan.FromMilliseconds(500))
            ;

        public override async Task RunAsync()
        {
            await base.RunAsync().ConfigureAwait(false);

            Console.WriteLine("PokeD.Server.PlayerBus.P3D");

            Waiter.WaitOne();
        }

        public override async Task StopAsync()
        {
            await base.StopAsync().ConfigureAwait(false);

            Waiter.Set();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Waiter.Dispose();
                Events.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}