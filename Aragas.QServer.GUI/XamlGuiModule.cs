using Aragas.QServer.GUI.ViewModels;
using Aragas.QServer.GUI.Views;
using Aragas.QServer.Hosting;
using Aragas.QServer.NetworkBus.Data;

using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.Modularity;

namespace Aragas.QServer.GUI
{
    [DependsOn(typeof(QServerModule))]
    public class XamlGuiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var services = context.Services;

            services.Configure<ServiceOptions>(o => o.Name = "GUI");

            //services.AddNpgSqlMetrics("Database", "Host=postgresql-minelib.aragas.org;Port=65432;Database=world;Username=minelib;Password=minelib");

            // Register all ViewModels.
            services.AddTransient<MainViewModel>();
            services.AddTransient<ServicesViewModel>();

            // Register all the Windows of the applications.
            services.AddTransient<MainWindow>();
            services.AddTransient<ServicesView>();
        }
    }
}