using MineLib.Server.Core;

using System;
using System.Threading.Tasks;

namespace MineLib.Server.MBus
{
    internal sealed class Program : BaseProgram
    {
        public static async Task Main(string[] args) => await Main<Program>(args).ConfigureAwait(false);

        private MBusListener Server { get; set; }

        public override async Task RunAsync()
        {
            await base.RunAsync().ConfigureAwait(false);

            Console.WriteLine($"MineLib.Server.MBus");

            Server = new MBusListener();
            Server.Start();

            Console.ReadLine();
            await StopAsync().ConfigureAwait(false);
        }

        public override async Task StopAsync()
        {
            Server.Stop();

            await base.StopAsync().ConfigureAwait(false);
        }
    }
}