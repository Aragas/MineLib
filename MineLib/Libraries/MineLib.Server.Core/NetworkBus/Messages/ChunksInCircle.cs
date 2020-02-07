using Aragas.QServer.NetworkBus.Messages;

namespace MineLib.Server.Core.NetworkBus.Messages
{
    public sealed class ChunksInCircleRequestMessage : JsonCompressedMessage
    {
        public override string Name => "minelib.server.worlbus.world.getchunks.circle.request";

        public int X { get; set; } = default!;
        public int Z { get; set; } = default!;
        public int Radius { get; set; } = default!;
    }
    public sealed class ChunksInCircleResponseMessage : JsonCompressedEnumerableMessage
    {
        public override string Name => "minelib.server.worlbus.world.getchunks.circle.response";

        public byte[] Data { get; set; } = default!;

        public ChunksInCircleResponseMessage() : base(false) { }
        public ChunksInCircleResponseMessage(bool isLast) : base(isLast) { }
    }
}