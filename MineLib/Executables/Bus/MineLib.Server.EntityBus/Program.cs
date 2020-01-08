using App.Metrics.Health;
using App.Metrics.Health.Checks.Sql;

using Aragas.QServer.Core;
using Aragas.QServer.Core.NetworkBus.Messages;

using MineLib.Server.Core;
using MineLib.Server.Core.NetworkBus.Messages;

using Npgsql;

using System;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace MineLib.Server.EntityBus
{
    internal sealed class Program : MineLibProgram
    {
        public static async Task Main(string[] args) => await Main<Program>(args).ConfigureAwait(false);

        public int EntityIDCounter = 0;

        private ManualResetEvent Waiter { get; } = new ManualResetEvent(false);
        private CompositeDisposable Events { get; } = new CompositeDisposable();

        public Program() : base(healthConfigure: ConfigureHealth)
        {
            Events.Add(BaseSingleton.Instance.SubscribeAndReply<ServicesPingMessage>(_ =>
                new ServicesPongMessage() { ServiceId = ProgramGuid, ServiceType = "EntityBus" }));

            Events.Add(BaseSingleton.Instance.SubscribeAndReply<GetNewEntityIdRequestMessage>(_ =>
                new GetNewEntityIdResponseMessage() { EntityId = ++EntityIDCounter }));
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