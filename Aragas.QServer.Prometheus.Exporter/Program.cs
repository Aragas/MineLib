using Aragas.QServer.Hosting.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Threading.Tasks;

namespace Aragas.QServer.Prometheus.Exporter
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunQServerAbpAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(services => services.AddApplication<PrometheusExporterModule>())
            .ConfigureWebHostDefaults(webBuilder => webBuilder
                .Configure(app => app.InitializeApplication())
                .UseKestrel()
            );
    }
}