using Aragas.QServer.NetworkBus.Messages;

using PokeD.Server.Core.Data;

using System;

namespace PokeD.Server.Core.NetworkBus.Messages
{
    public sealed class GetNewPlayerHandlerRequestMessage : JsonMessage
    {
        public override string Name => "poked.server.playerbus.player.playerhandler.getnew.request";

        public Guid PlayerId { get; set; } = default!;
        public PlayerType PlayerType { get; set; } = default!;
    }
    public sealed class GetNewPlayerHandlerResponseMessage : JsonMessage
    {
        public override string Name => "poked.server.playerbus.player.playerhandler.getnew.response";

        public Guid ServiceId { get; set; } = default!;
    }
}
