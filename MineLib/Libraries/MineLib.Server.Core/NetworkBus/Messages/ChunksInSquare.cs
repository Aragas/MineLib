using Aragas.QServer.NetworkBus.Messages;

namespace MineLib.Server.Core.NetworkBus.Messages
{
    public sealed class ChunksInSquareRequestMessage : JsonCompressedMessage
    {
        public override string Name => "minelib.server.worlbus.world.getchunks.square.request";

        public int X { get; set; } = default!;
        public int Z { get; set; } = default!;
        public int Radius { get; set; } = default!;
    }
    public sealed class ChunksInSquareResponseMessage : JsonCompressedEnumerableMessage
    {
        public override string Name => "minelib.server.worlbus.world.getchunks.square.response";

        public byte[] Data { get; set; } = default!;

        public ChunksInSquareResponseMessage() : base(false) { }
        public ChunksInSquareResponseMessage(bool isLast) : base(isLast) { }
    }
}