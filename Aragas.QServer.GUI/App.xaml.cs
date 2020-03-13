using Aragas.QServer.GUI.Views;
using Aragas.QServer.Hosting;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows;

using Volo.Abp;

namespace Aragas.QServer.GUI
{
    public partial class App
    {
        private static IAbpApplication _application = default!;
        public static IServiceProvider? ServiceProvider => _application.ServiceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            var application = QServerAbpApplicationFactory.Create<XamlGuiModule>();
            _application = application;
            var _ = application.RunQServerAsync();

            var window = ServiceProvider.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _application.Shutdown();
            _application.Dispose();

            base.OnExit(e);
        }
    }
}