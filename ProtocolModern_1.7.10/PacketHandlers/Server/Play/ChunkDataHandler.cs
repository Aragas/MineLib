using System.IO;

using Aragas.Core.Extensions;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Events.ReceiveEvents.Anvil;

using Org.BouncyCastle.Utilities.Zlib;

using ProtocolModern.Data;
using ProtocolModern.Packets.Client.Play;

namespace ProtocolModern.PacketHandlers.Server.Play
{
    public class ChunkDataHandler : ProtocolPacketHandler<ChunkDataPacket>
    {
        public override ProtobufPacket Handle(ChunkDataPacket packet)
        {
            var metadata = new ChunkColumnMetadata
            {
                Coordinates = new Coordinates2D(packet.ChunkX, packet.ChunkZ),
                PrimaryBitMap = packet.PrimaryBitmap
            };
            Context.ClientReceiveEvent(new ChunkEvent(ChunkExtra.ReadChunk(true, packet.GroundUpContinuous, metadata,
                    new ZInputStream(new MemoryStream(packet.CompressedData)).ReadFully())));

            return null;
        }
    }
}
