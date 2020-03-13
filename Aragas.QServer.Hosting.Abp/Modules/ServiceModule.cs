using Aragas.QServer.NetworkBus.Data;

using Microsoft.Extensions.DependencyInjection;

using System;

using Volo.Abp.Modularity;

namespace Aragas.QServer.Hosting.Modules
{
    public class ServiceModule : AbpModule
    {
        public Guid Uid { get; } = Guid.NewGuid();

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var services = context.Services;

            services.Configure<ServiceOptions>(o => o.Uid = Uid);
        }
    }
}