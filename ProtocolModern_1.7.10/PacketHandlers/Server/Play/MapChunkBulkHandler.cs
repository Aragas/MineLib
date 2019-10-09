using System.IO;

using Aragas.Core.Extensions;
using Aragas.Core.Packets;

using MineLib.Core.Events.ReceiveEvents.Anvil;

using Org.BouncyCastle.Utilities.Zlib;

using ProtocolModern.Data;
using ProtocolModern.Packets.Client.Play;

namespace ProtocolModern.PacketHandlers.Server.Play
{
    public class MapChunkBulkHandler : ProtocolPacketHandler<MapChunkBulkPacket>
    {
        public override ProtobufPacket Handle(MapChunkBulkPacket packet)
        {
            Context.ClientReceiveEvent(new ChunkArrayEvent(ChunkExtra.ReadChunkArray(packet.SkyLightSent, packet.MetaInformation,
                    new ZInputStream(new MemoryStream(packet.Data)).ReadFully())));

            return null;
        }
    }
}
