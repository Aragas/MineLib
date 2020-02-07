using Aragas.QServer.NetworkBus.Messages;

namespace MineLib.Server.Core.NetworkBus.Messages
{
    public sealed class GetNewEntityIdRequestMessage : JsonMessage
    {
        public override string Name => "minelib.server.entitybus.entity.getnewid.request";
    }
    public sealed class GetNewEntityIdResponseMessage : JsonMessage
    {
        public override string Name => "minelib.server.entitybus.entity.getnewid.response";

        public int EntityId { get; set; } = default!;
    }
}