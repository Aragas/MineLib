using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.IO;
using Aragas.QServer.Core.NetworkBus;

using MineLib.Core;
using MineLib.Core.Anvil;
using MineLib.Server.Core.NetworkBus.Messages;

using System.Collections.Generic;

namespace MineLib.Server.WorldBus
{
    public class WorldHandlerManager :
        IEnumerableMessageHandler<ChunksInSquareRequestMessage, ChunksInSquareResponseMessage>,
        IEnumerableMessageHandler<ChunksInCircleRequestMessage, ChunksInCircleResponseMessage>
    {
        public IWorldHandler WorldHandler { get; }

        public WorldHandlerManager(IWorldHandler worldHandler)
        {
            WorldHandler = worldHandler;
        }

        private IEnumerable<Chunk> GetChunksInSquareRequest(int x0, int z0, int radius)
        {
            for (int x = x0 - radius; x <= x0 + radius; x++)
                for (int z = z0 - radius; z <= z0 + radius; z++)
                    yield return WorldHandler.GetChunk(new Location2D(x, z));
        }
        public async IAsyncEnumerable<ChunksInSquareResponseMessage> HandleAsync(ChunksInSquareRequestMessage message)
        {
            foreach (var chunkElement in GetChunksInSquareRequest(message.X, message.Z, message.Radius).Detailed())
            {
                var serializer = new CompressedProtobufSerializer();
                serializer.Write(chunkElement.Value);
                yield return new ChunksInSquareResponseMessage(chunkElement.IsLast) { Data = serializer.GetData().ToArray() };
            }
        }

        private IEnumerable<Chunk> GetChunksInCircleRequest(int x0, int z0, int radius)
        {
            for (int x = x0 - radius; x <= x0 + radius; x++)
                for (int z = z0 - radius; z <= z0 + radius; z++)
                    if (((x - x0) * (x - x0)) + ((z - z0) * (z - z0)) <= radius * radius)
                        yield return WorldHandler.GetChunk(new Location2D(x, z));
        }
        public async IAsyncEnumerable<ChunksInCircleResponseMessage> HandleAsync(ChunksInCircleRequestMessage message)
        {
            foreach (var chunkElement in GetChunksInCircleRequest(message.X, message.Z, message.Radius).Detailed())
            {
                var serializer = new CompressedProtobufSerializer();
                serializer.Write(chunkElement.Value);
                yield return new ChunksInCircleResponseMessage(chunkElement.IsLast) { Data = serializer.GetData().ToArray() };
            }
        }
    }
}