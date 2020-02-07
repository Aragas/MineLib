﻿namespace Aragas.QServer.NetworkBus.Messages
{
    public sealed class AppMetricsPrometheusRequestMessage : JsonMessage
    {
        public override string Name => "services.metrics.prometheus.request";
    }
    public sealed class AppMetricsPrometheusResponseMessage : JsonMessage
    {
        public override string Name => "services.metrics.prometheus.response";

        public string Report { get; set; } = default!;

        public AppMetricsPrometheusResponseMessage() { }
        public AppMetricsPrometheusResponseMessage(string report) => Report = report;
    }
}