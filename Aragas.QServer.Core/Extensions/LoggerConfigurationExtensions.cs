using Aragas.QServer.Core.Serilog;

using Serilog.Configuration;
using Serilog.Exceptions;

using System;

namespace Serilog
{
    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration ConfigureSerilog(this LoggerConfiguration loggerConfiguration, Guid applicationUid)
        {
            return loggerConfiguration
                .Enrich.WithExceptionDetails()
                .Enrich.WithApplication(applicationUid)
                .Enrich.WithLogLevel();
        }

        public static LoggerConfiguration WithApplication(this LoggerEnrichmentConfiguration enrichmentConfiguration, Guid applicationUid)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With(new ApplicationNameEnricher(applicationUid));
        }
        public static LoggerConfiguration WithLogLevel(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With<LogLevelEnricher>();
        }
    }
}