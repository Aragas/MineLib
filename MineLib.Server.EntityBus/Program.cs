using MineLib.Server.Core;
using MineLib.Server.Core.Packets.EntityBus;

using System;
using System.Threading.Tasks;

namespace MineLib.Server.EntityBus
{
    internal sealed class Program : BaseProgram
    {
        public static async Task Main(string[] args) => await Main<Program>(args).ConfigureAwait(false);

        public int EntityIDCounter = 0;

        public override async Task RunAsync()
        {
            await base.RunAsync().ConfigureAwait(false);

            Console.WriteLine($"MineLib.Server.EntityBus");

            InternalBus.EntityBus.MessageReceived += EntityBusHandler_MessageReceived;

            Console.ReadLine();
            await StopAsync().ConfigureAwait(false);
        }

        public override async Task StopAsync()
        {
            await base.StopAsync().ConfigureAwait(false);

            InternalBus.EntityBus.MessageReceived -= EntityBusHandler_MessageReceived;
        }

        private void EntityBusHandler_MessageReceived(object sender, MBusMessageReceivedEventArgs args)
        {
            InternalBus.HandleRequest<EntityIDRequest, EntityIDResponse>(InternalBus.EntityBus, args, _ => new EntityIDResponse() { EntityID = ++EntityIDCounter });
        }

        public override void Dispose()
        {
            base.Dispose();

            InternalBus.EntityBus.Dispose();
        }
    }
}