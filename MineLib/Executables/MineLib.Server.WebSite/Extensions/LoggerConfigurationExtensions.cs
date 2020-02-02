using MineLib.Server.Heartbeat.Serilog;

using Serilog;
using Serilog.Configuration;
using Serilog.Exceptions;

using System;

namespace Serilog
{
    // Aragas.QServer.Core
    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration ConfigureSerilog(this LoggerConfiguration loggerConfiguration, Guid applicationUid)
        {
            return loggerConfiguration
                .Enrich.WithExceptionDetails()
                .Enrich.WithApplicationInfo(applicationUid)
                .Enrich.WithLogLevel();
        }

        public static LoggerConfiguration WithApplicationInfo(this LoggerEnrichmentConfiguration enrichmentConfiguration, Guid applicationUid)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With(new ApplicationInfoEnricher(applicationUid));
        }
        public static LoggerConfiguration WithLogLevel(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With<LogLevelEnricher>();
        }
    }
}