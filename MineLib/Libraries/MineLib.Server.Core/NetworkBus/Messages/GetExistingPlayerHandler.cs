using Aragas.QServer.NetworkBus.Messages;

using System;

namespace MineLib.Server.Core.NetworkBus.Messages
{
    public sealed class GetExistingPlayerHandlerRequestMessage : JsonMessage
    {
        public override string Name => "minelib.server.playerbus.player.playerhandler.getexisting.request";

        public Guid PlayerId { get; set; } = default!;
        public int ProtocolVersion { get; set; } = default!;
    }
    public sealed class GetExistingPlayerHandlerResponseMessage : JsonMessage
    {
        public override string Name => "minelib.server.playerbus.player.playerhandler.getexisting.response";

        public Guid? ServiceId { get; set; } = default!;
        public int State { get; set; } = default!;
    }
}