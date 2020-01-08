using Aragas.QServer.Core.NetworkBus.Messages;

using PokeD.Server.Core.Data;

using System;

namespace PokeD.Server.Core.NetworkBus.Messages
{
    public sealed class GetExistingPlayerHandlerRequestMessage : JsonMessage
    {
        public override string Name => "poked.server.playerbus.player.playerhandler.getexisting.request";

        public Guid PlayerId { get; set; } = default!;
        public PlayerType PlayerType { get; set; } = default!;
    }
    public sealed class GetExistingPlayerHandlerResponseMessage : JsonMessage
    {
        public override string Name => "poked.server.playerbus.player.playerhandler.getexisting.response";

        public Guid? ServiceId { get; set; } = default!;
        public int State { get; set; } = default!;
    }
}
