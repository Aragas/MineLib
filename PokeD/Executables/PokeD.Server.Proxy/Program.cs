using Aragas.QServer.Core;
using Aragas.QServer.Core.NetworkBus.Messages;
using PokeD.Server.Core;

using System;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace PokeD.Server.Proxy
{
    internal sealed class Program : PokeDProgram
    {
        public static int MaxConnections { get; set; } = 1000;
        public static int CurrentConnections { get; set; }
        public static string Name { get; set; } = "PokeD Docker";
        public static string Description { get; set; } = "Scalable C# Server";

        public static async Task Main(string[] args) => await Main<Program>(args).ConfigureAwait(false);

        public ProxyP3DListener PlayerListener { get; private set; }

        private ManualResetEvent Waiter { get; } = new ManualResetEvent(false);
        private CompositeDisposable Events { get; } = new CompositeDisposable();

        public Program()
        {
            Events.Add(BaseSingleton.Instance.SubscribeAndReply<ServicesPingMessage>(_ =>
                new ServicesPongMessage() { ServiceId = ProgramGuid, ServiceType = "Proxy" }));
            /*
            Events.Add(BaseSingleton.Instance.SubscribeAndReply<GetExistingPlayerHandlerRequestMessage>(
                message =>
                {
                    //if (PlayerHanlders.TryGetValue(message.PlayerId, out var playerHandler) && playerHandler.ProtocolVersion == message.ProtocolVersion)
                    //    return new GetExistingPlayerHandlerResponseMessage() { ServiceId = ProgramGuid, State = playerHandler.State!.Value };
                    //else
                    //    return new GetExistingPlayerHandlerResponseMessage() { ServiceId = null };
                    return default;
                }));
            Events.Add(BaseSingleton.Instance.SubscribeAndReplyToExclusive<GetNewPlayerHandlerRequestMessage, GetNewPlayerHandlerResponseMessage>(
                message =>
                {
                    return true;
                },
                message =>
                {
                    //var stuff = new PlayerHandler.PlayerHandler(message.PlayerId, message.ProtocolVersion);
                    //PlayerHanlders.TryAdd(message.PlayerId, stuff);
                    //
                    //return new GetNewPlayerHandlerResponseMessage() { ServiceId = ProgramGuid };
                    return default;
                }, ProgramGuid));
            */
        }

        public override async Task RunAsync()
        {
            await base.RunAsync().ConfigureAwait(false);

            Console.WriteLine($"MineLib.Server.Proxy");

            PlayerListener = new ProxyP3DListener();
            PlayerListener.Start();

            Waiter.WaitOne();
        }

        public override async Task StopAsync()
        {
            await base.StopAsync().ConfigureAwait(false);

            PlayerListener.Stop();

            Waiter.Set();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PlayerListener?.Stop();
                PlayerListener = null;

                Waiter.Dispose();
                Events.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}