using Aragas.QServer.NetworkBus.Messages;

using MineLib.Core;

using System;

namespace MineLib.Server.Core.NetworkBus.Messages
{
    public sealed class PlayerLookRequestMessage : JsonMessage
    {
        public override string Name => "minelib.server.playerbus.player.look.request";

        public Guid PlayerId { get; set; }
        public Look Look { get; set; }
    }
    public sealed class PlayerLookResponseMessage : JsonMessage
    {
        public override string Name => "minelib.server.playerbus.player.look.response";

        public bool IsCorrect { get; set; }
        public Look Look { get; set; }
    }
}
