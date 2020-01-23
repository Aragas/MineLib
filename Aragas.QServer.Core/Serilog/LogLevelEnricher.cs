using Serilog.Core;
using Serilog.Events;

namespace Aragas.QServer.Core.Serilog
{
    public class LogLevelEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Level", logEvent.Level.ToString()));
        }
    }
}