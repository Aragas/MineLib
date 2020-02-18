using Serilog.Core;
using Serilog.Events;

using System.Runtime.CompilerServices;

namespace Aragas.QServer.Logging.Serilog
{
    /// <summary>
    /// Provide a sared filter
    /// </summary>
    public class LokiWorkaroundEnricher : ILogEventEnricher
    {
        public const string LokiWorkaroundPropertyName = "workaround";

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateApplicationProperty(ILogEventPropertyFactory propertyFactory) =>
            propertyFactory.CreateProperty(LokiWorkaroundPropertyName, "workaround");

        private LogEventProperty? _cachedLokiWorkaroundProperty;


        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetLokiWorkaroundLogEventProperty(propertyFactory));
        }

        private LogEventProperty GetLokiWorkaroundLogEventProperty(ILogEventPropertyFactory propertyFactory) =>
            _cachedLokiWorkaroundProperty ??= CreateApplicationProperty(propertyFactory);
    }
}