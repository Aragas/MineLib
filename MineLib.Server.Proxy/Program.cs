using System;
using System.Threading;
using System.Threading.Tasks;

using MineLib.Server.Core;
using MineLib.Server.Core.Extensions;
using MineLib.Server.Core.Packets;

namespace MineLib.Server.Proxy
{
    /// <summary>
    /// Handles connection to Player
    /// Proxy the Player connection to MineLib.Server.PlayerHandler
    /// ---
    /// Proxy is used to hide the fact that we distribute Players to multiple
    /// PlayerHandler instances. It's like BungeeCord, but internally it's
    /// one big server, not multiple small servers.
    /// ---
    /// Responses to Server List Ping (The Netty should respond to both Legacy and Netty)
    /// ---
    /// When the Player starts to Login, find first available PlayerHandler and
    /// create a Proxy connection (Player<-->Proxy<-->PlayerHandler)
    /// Send every Login/Play state packet to PlayerHandler
    /// Else, abort connection with Player
    /// If Player disconnected, disconnect PlayerHandler too.
    /// ---
    /// Because of possible encryption and compression, do not attempt
    /// to read Login/Player state packets.
    /// ---
    /// If MineLib.Proxy is down, after creating a new MineLib.Proxy process,
    /// send to the PlayerHandlerBus via broadcast to get rid of every player
    /// ---
    /// To find an available PlayerHandler, send a broadcast message
    /// that you need a connection and (maybe?) wait for every response.
    /// Based on that response, either attempt to fill the biggest PlayerHandler
    /// or fill the smallest one (Player.Count context).
    /// </summary>
    internal sealed class Program : BaseProgram
    {
        public static int MaxConnections { get; set; } = 1000;
        public static int CurrentConnections { get; set; }
        public static string Description { get; set; } = "Scalable C# Server";

        public static async Task Main(string[] args) => await Main<Program>(args).ConfigureAwait(false);

        public PlayerNettyListener PlayerListener { get; private set; }

        public override async Task RunAsync()
        {
            await base.RunAsync().ConfigureAwait(false);

            Console.WriteLine($"MineLib.Server.Proxy");

            await Task.Factory.StartNew(CycleAsync, CancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default).ConfigureAwait(false);

            PlayerListener = new PlayerNettyListener();
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

        public override void Dispose()
        {
            base.Dispose();

            PlayerListener.Stop();
        }
    }
}