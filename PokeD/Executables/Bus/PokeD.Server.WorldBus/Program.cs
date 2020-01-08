using App.Metrics.Health;

using Aragas.QServer.Core;
using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.NetworkBus.Messages;
using PokeD.Server.Core;
using PokeD.Server.Core.Data;
using PokeD.Server.Core.NetworkBus.Messages;

using System;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace PokeD.Server.WorldBus
{
    internal sealed class Program : PokeDProgram
    {
        public static async Task Main(string[] args) => await Main<Program>(args).ConfigureAwait(false);

        private WorldService WorldService { get; } = new WorldService();

        private ManualResetEvent Waiter { get; } = new ManualResetEvent(false);
        private CompositeDisposable Events { get; } = new CompositeDisposable();

        public Program() : base(healthConfigure: ConfigureHealth)
        {
            Events.Add(BaseSingleton.Instance.SubscribeAndReply<ServicesPingMessage>(_ =>
                new ServicesPongMessage() { ServiceId = ProgramGuid, ServiceType = "WorldBus" }));
            Events.Add(BaseSingleton.Instance.SubscribeAndReply<GetWorldStateRequestMessage>(
                message =>
                {
                    return new GetWorldStateResponseMessage()
                    {
                        Season = WorldService.Season,
                        Weather = WorldService.Weather,
                        DoDayCycle = WorldService.DoDayCycle,
                        Time = WorldService.CurrentTime,
                        TimeSpanOffset = WorldService.TimeSpanOffset,
                        UseRealTime = WorldService.UseRealTime,
                    };
                }));
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

            WorldService.Start();

            Waiter.WaitOne();
        }

        public override async Task StopAsync()
        {
            await base.StopAsync().ConfigureAwait(false);

            WorldService.Stop();

            Waiter.Set();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                WorldService.Dispose();

                Waiter.Dispose();
                Events.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
