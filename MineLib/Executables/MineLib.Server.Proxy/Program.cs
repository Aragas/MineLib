using Aragas.QServer.Hosting;
using Aragas.QServer.Hosting.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Threading.Tasks;
using Volo.Abp;

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
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            MineLib.Server.Proxy.Extensions.PacketExtensions.Init();

            using var application = QServerAbpApplicationFactory.Create<ProxyModule>();
            await application.RunQServerAsync();

            //IEnvironmentSetup.SetEnvironment();
            //return CreateHostBuilder(args).Build().RunQServerAbpAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(services => services.AddApplication<ProxyModule>());
    }
}