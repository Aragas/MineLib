using Aragas.QServer.NetworkBus.Messages;

using System;

namespace MineLib.Server.Core.NetworkBus.Messages
{
    public sealed class GetNewPlayerHandlerRequestMessage : JsonMessage
    {
        public override string Name => "minelib.server.playerbus.player.playerhandler.getnew.request";

        public Guid PlayerId { get; set; } = default!;
        public int ProtocolVersion { get; set; } = default!;
    }
    public sealed class GetNewPlayerHandlerResponseMessage : JsonMessage
    {
        public override string Name => "minelib.server.playerbus.player.playerhandler.getnew.response";

        public Guid ServiceId { get; set; } = default!;
    }
}