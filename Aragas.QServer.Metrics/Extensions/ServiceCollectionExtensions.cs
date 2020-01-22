using App.Metrics;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
using App.Metrics.Health.Formatters.Ascii;
using App.Metrics.Health.Formatters.Json;

using Aragas.QServer.Health;
using Aragas.QServer.Metrics;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHealthCheckPublisher(this IServiceCollection services, Func<IHealthBuilder, IHealthBuilder>? additional = null)
        {
            services.AddSingleton<HealthCheck, CpuHealthCheck>();
            //services.AddSingleton<HealthCheck, NATSHealthCheck>();
            services.AddSingleton<HealthCheck, RamHealthCheck>();

            var builder = new HealthBuilder()
                .OutputHealth.Using(new HealthStatusTextOutputFormatter())
                .OutputHealth.Using(new HealthStatusJsonOutputFormatter());
            builder = additional?.Invoke(builder) ?? builder;
            builder.BuildAndAddTo(services);

            services.AddAppMetricsHealthPublishing();

            return services;
        }

        public static IServiceCollection AddPrometheusEndpoint(this IServiceCollection services, Func<IMetricsBuilder, IMetricsBuilder>? additional = null)
        {
            var metricsBuilder = new MetricsBuilder()
                .Configuration.Configure(options => options
                    //.AddServiceTag()
                    //.AddRuntimeTag()
                    .AddServerTag()
                    //.AddGitTag()
                    )

                .OutputMetrics.AsPrometheusPlainText()
                ;
            metricsBuilder = additional?.Invoke(metricsBuilder) ?? metricsBuilder;
            services.AddMetrics(metricsBuilder);

            services.AddMetricsReportingHostedService();

            return services;
        }

        public static IServiceCollection AddDefaultMetrics(this IServiceCollection services)
        {
            services.AddHostedService(sp => new StandardMetricsService(sp.GetRequiredService<IMetrics>(), sp.GetRequiredService<ILogger<StandardMetricsService>>()));
            services.AddHostedService(sp => new CpuUsageMetricsService(sp.GetRequiredService<IMetrics>(), sp.GetRequiredService<ILogger<CpuUsageMetricsService>>()));
            services.AddHostedService(sp => new MemoryUsageMetricsService(sp.GetRequiredService<IMetrics>(), sp.GetRequiredService<ILogger<MemoryUsageMetricsService>>()));

            return services;
        }

        public static IServiceCollection AddNpgSqlMetrics(this IServiceCollection services, string connectionName, string connectionString)
        {
            services.AddSingleton<IHostedService>(sp => new PostgreSQLMetricsService(
                sp.GetRequiredService<IMetrics>(),
                connectionName,
                connectionString,
                sp.GetRequiredService<ILogger<PostgreSQLMetricsService>>()));

            return services;
        }

        public static IServiceCollection AddSqlServerMetrics(this IServiceCollection services, string connectionName, string connectionString)
        {
            services.AddSingleton<IHostedService>(sp => new MSSQLMetricsService(
                sp.GetRequiredService<IMetrics>(),
                connectionName,
                connectionString,
                sp.GetRequiredService<ILogger<MSSQLMetricsService>>()));

            return services;
        }
    }
}