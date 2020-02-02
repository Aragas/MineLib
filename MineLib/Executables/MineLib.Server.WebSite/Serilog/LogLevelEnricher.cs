using Serilog.Core;
using Serilog.Events;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MineLib.Server.Heartbeat.Serilog
{
    // Aragas.QServer.Core
    public sealed class LogLevelEnricher : ILogEventEnricher
    {
        public const string LevelPropertyName = "Level";

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateLevelProperty(ILogEventPropertyFactory propertyFactory, LogEventLevel level) =>
            propertyFactory.CreateProperty(LevelPropertyName, level.ToString());

        private Dictionary<LogEventLevel, LogEventProperty> _cachedLevelProperty = new Dictionary<LogEventLevel, LogEventProperty>();

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetLevelLogEventProperty(propertyFactory, logEvent.Level));
        }

        private LogEventProperty GetLevelLogEventProperty(ILogEventPropertyFactory propertyFactory, LogEventLevel level)
        {
            if (!_cachedLevelProperty.ContainsKey(level))
                _cachedLevelProperty[level] = CreateLevelProperty(propertyFactory, level);

            return _cachedLevelProperty[level];
        }
    }
}