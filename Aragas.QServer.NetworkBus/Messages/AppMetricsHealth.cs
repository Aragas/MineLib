﻿namespace Aragas.QServer.NetworkBus.Messages
{
    public sealed class AppMetricsHealthRequestMessage : JsonMessage
    {
        public override string Name => "services.metrics.health.request";
    }
    public sealed class AppMetricsHealthResponseMessage : JsonMessage
    {
        public override string Name => "services.metrics.health.response";

        public string Report { get; set; } = default!;

        public AppMetricsHealthResponseMessage() { }
        public AppMetricsHealthResponseMessage(string report) => Report = report;
    }
}