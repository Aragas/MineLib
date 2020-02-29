using Aragas.QServer.NetworkBus.Data;

using I18Next.Net.Backends;
using I18Next.Net.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using MineLib.Server.Core;
using MineLib.Server.Proxy.BackgroundServices;
using MineLib.Server.Proxy.Data;

using System.Net;
using System.Threading.Tasks;

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
    public sealed class Program
    {
        public static async Task Main(string[] args)
        {
            //ServicePointManager.UseNagleAlgorithm = false;
            MineLib.Server.Proxy.Extensions.PacketExtensions.Init();
            await MineLibHostProgram.Main<Program>(CreateHostBuilder, BeforeRun, args);
        }

        public static IHostBuilder CreateHostBuilder(IHostBuilder hostBuilder) => hostBuilder
            // Options
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<ServiceOptions>(o => o.Name = "Proxy");
                services.Configure<MineLibOptions>(hostContext.Configuration.GetSection("MineLib"));

                services.AddSingleton<ServerInfo>();
                services.AddSingleton<ClassicServerInfo>();
            })

            // Localization
            .ConfigureServices(services =>
            {
                services.AddI18NextLocalization(i18N => i18N.AddBackend(new JsonFileBackend("locales")));
            })

            // Metrics
            .ConfigureServices(services =>
            {
                services.AddDotNetRuntimeStats();
            })

            // Netty Listener
            .ConfigureServices(services  =>
            {
                services.AddHostedService<ProxyNettyListenerService>();
                services.AddHostedService<ProxyClassicListenerService>();
            })

            .UseConsoleLifetime();

        private static void BeforeRun(IHost host)
        {
            var serviceOptions = host.Services.GetRequiredService<IOptions<ServiceOptions>>().Value;
        }
    }
}