using Aragas.QServer.Core.NetworkBus.Messages;

using System;
using System.Numerics;

namespace MineLib.Server.Core.NetworkBus.Messages
{
    public sealed class PlayerPositionRequestMessage : JsonMessage
    {
        public override string Name => "minelib.server.playerbus.player.position.request";

        public Guid PlayerId { get; set; }
        public Vector3 Position { get; set; }
    }
    public sealed class PlayerPositionResponseMessage : JsonMessage
    {
        public override string Name => "minelib.server.playerbus.player.position.response";

        public bool IsCorrect { get; set; }
        public Vector3 Position { get; set; }
    }
}