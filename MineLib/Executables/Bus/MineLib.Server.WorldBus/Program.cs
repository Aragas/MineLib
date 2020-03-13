using Aragas.QServer.Hosting;
using Aragas.QServer.Hosting.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Threading.Tasks;

using Volo.Abp;


namespace MineLib.Server.WorldBus
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var application = QServerAbpApplicationFactory.Create<WorldBusModule>();
            await application.RunQServerAsync();
            //await CreateHostBuilder(args).Build().RunQServerAbpAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(services => services.AddApplication<WorldBusModule>());
    }
}