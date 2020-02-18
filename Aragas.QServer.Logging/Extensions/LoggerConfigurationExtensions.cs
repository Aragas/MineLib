﻿using Aragas.QServer.Logging.Serilog;

using Serilog.Configuration;
using Serilog.Sinks.Loki;

using System;

namespace Serilog
{
    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration LokiHttp(this LoggerSinkConfiguration sinkConfiguration, string serverUrl)
            => sinkConfiguration.LokiHttp(new NoAuthCredentials(serverUrl));
        public static LoggerConfiguration LokiHttp(this LoggerSinkConfiguration sinkConfiguration, string serverUrl, string username, string password)
            => sinkConfiguration.LokiHttp(new BasicAuthCredentials(serverUrl, username, password));


        public static LoggerConfiguration WithApplicationInfo(this LoggerEnrichmentConfiguration enrichmentConfiguration, Guid applicationUid)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With(new ApplicationInfoEnricher(applicationUid));
        }
        public static LoggerConfiguration WithLokiWorkaround(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With<LokiWorkaroundEnricher>();
        }
    }
}