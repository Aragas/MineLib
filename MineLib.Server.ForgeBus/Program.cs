using MineLib.Server.Core;

using System;
using System.Threading.Tasks;

namespace MineLib.Server.ForgeBus
{
    // This shit should do dark jni shit and bridge the whole Forge API and our bus system
    // Basically, here we run the plugin with initialization and shit. I think that we only need to retranslate
    // PluginMessage packet from Protocol to ForgeBus
    internal sealed class Program : BaseProgram
    {
        public static async Task Main(string[] args) => await Main<Program>(args).ConfigureAwait(false);

        public override async Task RunAsync()
        {
            await base.RunAsync().ConfigureAwait(false);

            Console.WriteLine($"MineLib.Server.ForgeBus");

            InternalBus.ForgeBus.MessageReceived += (this, ForgeBus_MessageReceived);

            Console.ReadLine();
            await StopAsync().ConfigureAwait(false);
        }

        public override async Task StopAsync()
        {
            await base.StopAsync().ConfigureAwait(false);

            InternalBus.ForgeBus.MessageReceived -= ForgeBus_MessageReceived;
        }

        private void ForgeBus_MessageReceived(object? sender, MBusMessageReceivedEventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                InternalBus.ForgeBus.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
