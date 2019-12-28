using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.Packets;

using PokeD.Server.Core;
using PokeD.Server.Proxy;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MineLib.Server.Proxy
{
    internal sealed class Program : PokeDProgram
    {
        public static int MaxConnections { get; set; } = 1000;
        public static int CurrentConnections { get; set; }
        public static string Description { get; set; } = "Scalable C# Server";

        public static async Task Main(string[] args) => await Main<Program>(args).ConfigureAwait(false);

        public PlayerP3DListener PlayerListener { get; private set; }

        public override async Task RunAsync()
        {
            await base.RunAsync().ConfigureAwait(false);

            Console.WriteLine($"MineLib.Server.Proxy");

            //await Task.Factory.StartNew(CycleAsync, CancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default).ConfigureAwait(false);

            PlayerListener = new PlayerP3DListener();
            PlayerListener.Start();

            Console.ReadLine();
            await StopAsync().ConfigureAwait(false);
        }

        public override async Task StopAsync()
        {
            await base.StopAsync().ConfigureAwait(false);

            PlayerListener.Stop();
        }

        private async Task CycleAsync()
        {
            while (!CancellationTokenSource.IsCancellationRequested)
            {
                InternalBus.PlayerBus.SendPacket(new PingPacket() { GUID = Guid.NewGuid() });
                await Task.Delay(1000).ConfigureAwait(false);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //PlayerListener.Stop();
            }

            base.Dispose(disposing);
        }
    }
}