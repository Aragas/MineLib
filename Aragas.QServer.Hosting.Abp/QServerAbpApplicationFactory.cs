using System;

using Volo.Abp;
using Volo.Abp.Modularity;

namespace Aragas.QServer.Hosting
{
    public static class QServerAbpApplicationFactory
    {
        public static IAbpApplicationWithInternalServiceProvider Create<TStartupModule>(Action<AbpApplicationCreationOptions>? optionsAction = null)
            where TStartupModule : IAbpModule
        {
            return Create(typeof(TStartupModule), optionsAction);
        }

        public static IAbpApplicationWithInternalServiceProvider Create(Type startupModuleType, Action<AbpApplicationCreationOptions>? optionsAction = null)
        {
            return new QServerAbpApplication(startupModuleType, optionsAction);
        }

        /*
        public static IAbpApplicationWithExternalServiceProvider Create<TStartupModule>(IServiceCollection services, Action<AbpApplicationCreationOptions>? optionsAction = null)
            where TStartupModule : IAbpModule
        {
            return Create(typeof(TStartupModule), services, optionsAction);
        }

        public static IAbpApplicationWithExternalServiceProvider Create(Type startupModuleType, IServiceCollection services, Action<AbpApplicationCreationOptions>? optionsAction = null)
        {
            return new QServerAbpApplication(startupModuleType, services, optionsAction);
        }
        */
    }
}