using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;

using Volo.Abp;
using Volo.Abp.Modularity;

namespace Aragas.QServer.Hosting
{
    internal class QServerAbpApplication : IAbpApplicationWithInternalServiceProvider, IEnvironmentSetup
    {
        private readonly IHost _host;
        private readonly IAbpApplicationWithExternalServiceProvider _application;

        public IReadOnlyList<IAbpModuleDescriptor> Modules => _application.Modules;
        public Type StartupModuleType => _application.StartupModuleType;
        public IServiceCollection Services => _application.Services;
        public IServiceProvider ServiceProvider => _application.ServiceProvider;

        public QServerAbpApplication(Type startupModuleType, Action<AbpApplicationCreationOptions>? optionsAction = null)
        {
            IEnvironmentSetup.SetEnvironment();

            _host = new HostBuilder().ConfigureServices(services => AbpApplicationFactory.Create(startupModuleType, services, optionsAction)).Build();
            _application = _host.Services.GetRequiredService<IAbpApplicationWithExternalServiceProvider>();
        }

        public void Initialize() => _application.Initialize(_host.Services);

        public void Shutdown() => _application.Shutdown();

        public void Dispose() => _application.Dispose();
    }
}