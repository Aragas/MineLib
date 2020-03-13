using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.Modularity;

namespace Aragas.QServer.Hosting.Modules
{
    public class MetricsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var services = context.Services;

            services.AddPrometheusEndpoint();
            services.AddBuiltInMetrics();
            services.AddDotNetRuntimeMetrics();
        }
    }
}