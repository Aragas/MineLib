using App.Metrics;
using App.Metrics.DotNetRuntime;
using Aragas.QServer.GUI.ViewModels;
using Aragas.QServer.GUI.Views;
using Aragas.QServer.Hosting;
using Aragas.QServer.NetworkBus.Data;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Windows;

namespace Aragas.QServer.GUI
{
    public partial class App
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        private IHost _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            var _ = QServerHostProgram.Main<App>(hostBuilderFunc: CreateHostBuilder, beforeRunAction: BeforeRun);

            var window = ServiceProvider.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync(TimeSpan.FromSeconds(5));
            _host.Dispose();

            base.OnExit(e);
        }

        public static IHostBuilder CreateHostBuilder(IHostBuilder hostBuilder) => hostBuilder
            .ConfigureServices(services =>
            {
                services.Configure<ServiceOptions>(o => o.Name = "GUI");
            })
            .ConfigureServices(services =>
            {
                // Register all ViewModels.
                services.AddTransient<MainViewModel>();
                services.AddTransient<ServicesViewModel>();

                // Register all the Windows of the applications.
                services.AddTransient<MainWindow>();
                services.AddTransient<ServicesView>();
            })
            .ConfigureServices(services =>
            {
                //services.AddDotNetRuntimeStats();
            });

        private void BeforeRun(IHost host)
        {
            _host = host;
            ServiceProvider = host.Services;
        }
    }
}