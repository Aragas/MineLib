using Aragas.QServer.NetworkBus;
using Aragas.QServer.NetworkBus.Data;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using MineLib.Server.Core;

using System.Threading.Tasks;

namespace MineLib.Server.PlayerBus
{
    /// <summary>
    /// Handles Login/Play state packets
    /// ---
    /// MineLib abstracts actions like Block placement/destructions,
    /// Player damaging, Chest access, Chat IO, etc,
    /// so multiple Protocols with different implementations
    /// can do those actions without problems.
    /// ---
    /// PlayerHandler uses multiple Protocols, so we can support
    /// multiple client versions. This way tho, we will break
    /// Mod support, in case when between 1.7.10 and 1.12 major changes
    /// were made. In such cases, ban specific versions.
    /// ---
    /// In the future, I hope to make Java Forge API mod support.
    /// With this architecture, we can assign each mod it's own process,
    /// so maybe it won't be slow as fuck and will actually work
    /// ---
    /// The Java Forge API will interchange data via a ModAPIBus, that will
    /// be implemented for both EntityBus and WorldBus.
    /// This will be another slowdown, but at least this way will give us another
    /// possibility with C#<-->Java interaction - Serialization.
    /// So, basically, the first idea with Java Forge API was to host a C# converter,
    /// that will take the jar files and do shit via JNI or similar,
    /// but now we can actually host the Java processes and because of
    /// the class serialization via network, they won't even know that
    /// they are communicating with another language.
    /// We could use protobuf serialization
    /// ---
    /// I think we should either abstract Java Forge API or abstract all Minecraft
    /// classes that the API uses. I would prefer first ofc.
    /// So we will basically replace the Forge API with out own imlementation
    /// instead of providing Forge API our own Minecraft implementation.
    /// But I think that the Forge API gives the modder the ability to
    /// use Minecraft classes, so it won't work then.
    /// It does provide Minecraft classes. It seems that the best solution will be
    /// to provide Forge API implementation and Minecraft...
    /// http://www.wuppy29.com/minecraft/modding-tutorials/forge-modding-1-8/
    /// We can implemet first such basic functionality like registering the mod
    /// and calling the initialization stages (new recepie suport etc).
    /// The best approach to start implementing the Forge APi is to follow
    /// the simple tutorial files. I will need tho a Java developer
    /// that will help me write the Java proccess, which will serialize
    /// all the classes that will be needed to interact with ModAPIBus
    /// ---
    /// </summary>
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await MineLibHostProgram.Main<Program>(CreateHostBuilder, BeforeRun, args);
        }

        public static IHostBuilder CreateHostBuilder(IHostBuilder hostBuilder) => hostBuilder
            // Options
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<ServiceOptions>(o => o.Name = "PlayerBus");
            })

            .ConfigureServices(services =>
            {
                services.AddSingleton<PlayerHandlerManager>();
                services.AddSingleton<PlayerTest>();
            })

            // Metrics
            .ConfigureServices(services =>
            {
                services.AddDotNetRuntimeStats();
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddNpgSqlMetrics("Database", hostContext.Configuration["PostgreSQLConnectionString"]);
            })

            .UseConsoleLifetime();

        private static void BeforeRun(IHost host)
        {
            var serviceOptions = host.Services.GetRequiredService<IOptions<ServiceOptions>>().Value;
            var subscriptionStorage = host.Services.GetRequiredService<SubscriptionStorage>();

            subscriptionStorage.HandleGetExistingPlayerHandler<PlayerHandlerManager>();
            subscriptionStorage.HandleGetNewPlayerHandler<PlayerHandlerManager>(serviceOptions.Uid);

            subscriptionStorage.HandlePlayerPosition<PlayerTest>();
            subscriptionStorage.HandlePlayerLook<PlayerTest>();
        }
    }
}