using Aragas.Network;
using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using MineLib.Protocol.Packets;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MineLib.Protocol.Protocol
{
    public class MinecraftEnumFactory<TPacketType> : BasePacketFactory<TPacketType, VarInt, ProtobufSerializer, ProtobufDeserializer> 
        where TPacketType : MinecraftEnumPacket
    {
        private static Dictionary<Type, VarInt> IDTypeFromPacketType { get; } = new Dictionary<Type, VarInt>();
        private static Dictionary<VarInt, Func<TPacketType>> Packets { get; } = new Dictionary<VarInt, Func<TPacketType>>();
        private static bool IsInizialized { get; set; }
        private static readonly object _lockObject = new object();

        public MinecraftEnumFactory()
        {
            if (!IsInizialized)
            {
                lock (_lockObject) // Thread safe
                {
                    if (!IsInizialized)
                    {
                        IsInizialized = true;

                        var whereToFindPackets = AppDomain.CurrentDomain.GetAssemblies().Where(asm => !asm.IsDynamic);

                        var list = whereToFindPackets.SelectMany(asm => asm.ExportedTypes.Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(TPacketType)))).ToList();
                        foreach (var packetType in list)
                        {
                            if (packetType != null && ActivatorCached.CreateInstance(packetType) is TPacketType p)
                            {
                                Packets.Add(p.ID, () => (TPacketType) ActivatorCached.CreateInstance(packetType));
                                IDTypeFromPacketType.Add(p.GetType(), p.ID);
                            }
                            /*
                            if (packetType != null)
                            {
                                var p = ActivatorCached.CreateInstanceGeneric<TPacketType>(packetType);
                                Packets.Add(p.ID, () => ActivatorCached.CreateInstanceGeneric<TPacketType>(packetType));
                                IDTypeFromPacketType.Add(p.GetType(), p.ID);
                            }
                            */
                        }
                    }
                }
            }
        }

        public override TPacketType Create(VarInt packetID) => Packets.TryGetValue(packetID, out var packetConstructor) ? packetConstructor() : null;
        public override TPacketTypeCustom Create<TPacketTypeCustom>() => Packets.TryGetValue(IDTypeFromPacketType[typeof(TPacketTypeCustom)], out var packetConstructor) ? (TPacketTypeCustom) packetConstructor() : null;
        public override TPacketTypeCustom Create<TPacketTypeCustom>(Func<TPacketTypeCustom> initializer) => initializer();
    }
}