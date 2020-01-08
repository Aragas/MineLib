using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
using App.Metrics.Health.Formatters.Ascii;
using App.Metrics.Health.Formatters.Json;

using Aragas.QServer.Core.AppMetrics;
using Aragas.QServer.Core.Extensions;

using NATS.Client;

using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace Aragas.QServer.Core
{
    public partial class BaseProgram
    {
        protected static CounterOptions ExceptionCounter { get; } = new CounterOptions()
        {
            Name = "Total Exception Count",
            MeasurementUnit = Unit.Errors,
        };


        protected IMetricsRoot Metrics { get; }
        protected IHealthRoot Health { get; }

        private IDisposable AppMetricsPrometheusEvent { get; }
        private IDisposable AppMetricsHealthEvent { get; }

        public BaseProgram(Func<IMetricsBuilder, IMetricsBuilder>? metricsConfigure = null, Func<IHealthBuilder, IHealthBuilder>? healthConfigure = null)
        {
            var metricsBuilder = new MetricsBuilder()
                .Configuration.Configure(options => options
                    .AddMachineNameTag()
                    .AddAppTag()
                    .AddUUIDTag(ProgramGuid)
                    .AddEnvTag())

                .OutputEnvInfo.AsPlainText()
                .OutputEnvInfo.AsJson()

                .OutputMetrics.AsJson()
                .OutputMetrics.AsPlainText()
                .OutputMetrics.AsPrometheusPlainText()
                .OutputMetrics.AsPrometheusProtobuf()

                .SampleWith.ForwardDecaying()

                .TimeWith.StopwatchClock();
            if (metricsConfigure != null)
                metricsBuilder = metricsConfigure(metricsBuilder);
            Metrics = metricsBuilder.Build();

            var healthBuilder = new HealthBuilder()
                .HealthChecks.AddNatsConnectivityCheck("NATS Connection", ConnectionFactory.GetDefaultOptions().SetDefaultArgs(), 300)
                .HealthChecks.AddSystemMemoryHealthCheck("System Memory")
                .HealthChecks.AddCpuUsageHealthCheck("CPU Usage")
                .OutputHealth.Using(new HealthStatusTextOutputFormatter())
                .OutputHealth.Using(new HealthStatusJsonOutputFormatter());
            if (healthConfigure != null)
                healthBuilder = healthConfigure(healthBuilder);
            Health = healthBuilder.Build();

            AppMetricsPrometheusEvent = BaseSingleton.Instance.SubscribeAndReply(new AppMetricsPrometheusHandler(Metrics), ProgramGuid);
            AppMetricsHealthEvent = BaseSingleton.Instance.SubscribeAndReply(new AppMetricsHealthHandler(Health), ProgramGuid);

            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;

            Metrics.Measure.Counter.Increment(ExceptionCounter);
            Metrics.Measure.Counter.Decrement(ExceptionCounter);


        }
        private void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            Metrics.Measure.Counter.Increment(ExceptionCounter);
#if DEBUG
            var stackTrace = new StackTrace(true);
            if(!(e.Exception is NATSException) && (e.Exception is NullReferenceException && e.Exception.Source != "NATS.Client"))
                ;
#endif
        }
    }
}