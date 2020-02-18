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

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateApplicationProperty(ILogEventPropertyFactory propertyFactory) =>
            propertyFactory.CreateProperty(ApplicationPropertyName, Assembly.GetEntryAssembly()?.GetName().Name ?? "UNKNOWN");

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateApplicationUidProperty(ILogEventPropertyFactory propertyFactory, Guid applicationUid) =>
            propertyFactory.CreateProperty(ApplicationUidPropertyName, applicationUid);

        private readonly Guid _applicationUid;

        private LogEventProperty? _cachedApplicationNameProperty;
        private LogEventProperty? _cachedApplicationUidProperty;

        public ApplicationInfoEnricher(Guid applicationUid)
        {
            _applicationUid = applicationUid;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetApplicationLogEventProperty(propertyFactory));
            logEvent.AddPropertyIfAbsent(GetApplicationUidLogEventProperty(propertyFactory));
        }

        private LogEventProperty GetApplicationLogEventProperty(ILogEventPropertyFactory propertyFactory) =>
            _cachedApplicationNameProperty ??= CreateApplicationProperty(propertyFactory);

        private LogEventProperty GetApplicationUidLogEventProperty(ILogEventPropertyFactory propertyFactory) =>
            _cachedApplicationUidProperty ??= CreateApplicationUidProperty(propertyFactory, _applicationUid);
    }
}