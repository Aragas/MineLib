using Serilog.Core;
using Serilog.Events;

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Aragas.QServer.Logging.Serilog
{
    public class ApplicationInfoEnricher : ILogEventEnricher
    {
        public const string ApplicationPropertyName = "Application";
        public const string ApplicationUidPropertyName = "ApplicationUid";
        public const string EnvironmentPropertyName = "Environment";

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateApplicationProperty(ILogEventPropertyFactory propertyFactory) =>
            propertyFactory.CreateProperty(ApplicationPropertyName, Assembly.GetEntryAssembly()?.GetName().Name ?? "UNKNOWN");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateApplicationUidProperty(ILogEventPropertyFactory propertyFactory, Guid applicationUid) =>
            propertyFactory.CreateProperty(ApplicationUidPropertyName, applicationUid);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateEnvironmentProperty(ILogEventPropertyFactory propertyFactory) =>
            propertyFactory.CreateProperty(EnvironmentPropertyName, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "UNKNOWN");

        private readonly Guid _applicationUid;

        private LogEventProperty? _cachedApplicationNameProperty;
        private LogEventProperty? _cachedApplicationUidProperty;
        private LogEventProperty? _cachedEnvironmentProperty;

        public ApplicationInfoEnricher(Guid applicationUid)
        {
            _applicationUid = applicationUid;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetApplicationLogEventProperty(propertyFactory));
            logEvent.AddPropertyIfAbsent(GetApplicationUidLogEventProperty(propertyFactory));
            logEvent.AddPropertyIfAbsent(GetEnvironmentLogEventProperty(propertyFactory));
        }

        private LogEventProperty GetApplicationLogEventProperty(ILogEventPropertyFactory propertyFactory) =>
            _cachedApplicationNameProperty ??= CreateApplicationProperty(propertyFactory);

        private LogEventProperty GetApplicationUidLogEventProperty(ILogEventPropertyFactory propertyFactory) =>
            _cachedApplicationUidProperty ??= CreateApplicationUidProperty(propertyFactory, _applicationUid);

        private LogEventProperty GetEnvironmentLogEventProperty(ILogEventPropertyFactory propertyFactory) =>
            _cachedEnvironmentProperty ??= CreateEnvironmentProperty(propertyFactory);
    }
}