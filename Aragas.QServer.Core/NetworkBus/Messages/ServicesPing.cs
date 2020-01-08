using System;

namespace Aragas.QServer.Core.NetworkBus.Messages
{
    public sealed class ServicesPingMessage : JsonMessage
    {
        public override string Name => "services.ping";
    }
    public sealed class ServicesPongMessage : JsonMessage
    {
        public override string Name => "services.pong";

        public string ServiceType { get; set; } = default!;
        public Guid ServiceId { get; set; } = default!;
    }
}