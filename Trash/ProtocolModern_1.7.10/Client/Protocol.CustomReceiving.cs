using System;
using System.Collections.Generic;

using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Events;
using MineLib.Core.Events.ReceiveEvents.Anvil;

namespace ProtocolModern.Client
{
    public sealed partial class Protocol
    {
        public void ClientReceiveEvent(ReceiveEvent evnt) { Client.DoReceiveEvent(evnt); }


        private Dictionary<Type, List<Action<ProtobufPacket>>> CustomPacketHandlers { get; } = new Dictionary<Type, List<Action<ProtobufPacket>>>();
        private static Action<ProtobufPacket> TransformReceiving<TPacketType>(Action<TPacketType> action) where TPacketType : ProtobufPacket => packet => action((TPacketType) packet);

        public override void RegisterCustomReceiving<TPacketType>(Action<TPacketType> func)
        {
            var packetType = typeof(TPacketType);

            if (CustomPacketHandlers.ContainsKey(packetType))
                CustomPacketHandlers[packetType].Add(TransformReceiving(func));
            else
                CustomPacketHandlers.Add(packetType, new List<Action<ProtobufPacket>> { TransformReceiving(func) });
        }
        public override void DeregisterCustomReceiving<TPacketType>(Action<TPacketType> func)
        {
            var packetType = typeof (TPacketType);

            if (CustomPacketHandlers.ContainsKey(packetType))
                CustomPacketHandlers[packetType].Remove(TransformReceiving(func));
        }

        public override void DoCustomReceiving<TPacketType>(TPacketType packet)
        {
            var packetType = packet.GetType();

            List<Action<ProtobufPacket>> list;
            if(CustomPacketHandlers.TryGetValue(packetType, out list))
                foreach (var func in list)
                    func(packet);
        }


        #region Anvil

        private void OnChunk(Chunk chunk) { Client.DoReceiveEvent(new ChunkEvent(chunk)); }

        private void OnChunkArray(Chunk[] chunks) { Client.DoReceiveEvent(new ChunkArrayEvent(chunks)); }

        private void OnBlockChange(Position location, int block) { Client.DoReceiveEvent(new BlockChangeEvent(location, block)); }

        private void OnMultiBlockChange(Coordinates2D chunkLocation, BlockPosition[] records) { Client.DoReceiveEvent(new MultiBlockChangeEvent(chunkLocation, records)); }

        private void OnBlockAction(Position location, int block, object blockAction) { Client.DoReceiveEvent(new BlockActionEvent(location, block, blockAction)); }

        private void OnBlockBreakAction(int entityID, Position location, byte stage) { Client.DoReceiveEvent(new BlockBreakActionEvent(entityID, location, stage)); }

        #endregion
    }
}
