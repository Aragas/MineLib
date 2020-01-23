using Serilog.Core;
using Serilog.Events;

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Aragas.QServer.Core.Serilog
{
    public class ApplicationNameEnricher : ILogEventEnricher
    {
        private Guid _applicationUid;

        private LogEventProperty _cachedApplicationNameProperty;
        private LogEventProperty _cachedApplicationUidProperty;

        public const string ApplicationNamePropertyName = "ApplicationName";
        public const string ApplicationUidPropertyName = "ApplicationUid";

        public ApplicationNameEnricher(Guid applicationUid)
        {
            _applicationUid = applicationUid;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetApplicationNameLogEventProperty(propertyFactory));
            logEvent.AddPropertyIfAbsent(GetApplicationUidLogEventProperty(propertyFactory));
        }

        private LogEventProperty GetApplicationNameLogEventProperty(ILogEventPropertyFactory propertyFactory)
        {
            // Don't care about thread-safety, in the worst case the field gets overwritten and one property will be GCed
            return _cachedApplicationNameProperty ?? (_cachedApplicationNameProperty = CreateApplicationNameProperty(propertyFactory));
        }
        // Qualify as uncommon-path
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateApplicationNameProperty(ILogEventPropertyFactory propertyFactory)
        {
            var applicationName = Assembly.GetEntryAssembly().GetName().Name;
            return propertyFactory.CreateProperty(ApplicationNamePropertyName, applicationName);
        }

        private LogEventProperty GetApplicationUidLogEventProperty(ILogEventPropertyFactory propertyFactory)
        {
            // Don't care about thread-safety, in the worst case the field gets overwritten and one property will be GCed
            return _cachedApplicationUidProperty ?? (_cachedApplicationUidProperty = CreateApplicationUidProperty(propertyFactory, _applicationUid));
        }
        // Qualify as uncommon-path
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateApplicationUidProperty(ILogEventPropertyFactory propertyFactory, Guid applicationUid)
        {
            return propertyFactory.CreateProperty(ApplicationUidPropertyName, applicationUid);
        }
    }
}