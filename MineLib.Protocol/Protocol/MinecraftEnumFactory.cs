using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Aragas.Network;
using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using MineLib.Protocol.Packets;

namespace MineLib.Protocol.Protocol
{
    public class MinecraftEnumFactory<TPacketType, TEnum> : BasePacketFactory<TPacketType, VarInt, ProtobufSerializer, ProtobufDeserializer> 
        where TPacketType : MinecraftPacket 
        where TEnum : Enum
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

                        foreach (var packetType in whereToFindPackets.SelectMany(asm => asm.ExportedTypes.Where(type => type.GetTypeInfo().IsSubclassOf(typeof(TPacketType)))))
                        {
                            var p = ActivatorCached.CreateInstance(packetType) as TPacketType;
                            Packets.Add(p.ID, packetType != null ? (Func<TPacketType>)(() => ActivatorCached.CreateInstance(packetType) as TPacketType) : null);
                            IDTypeFromPacketType.Add(p.GetType(), p.ID);
                        }
                    }
                }
            }
        }

        public override TPacketType Create(VarInt packetID) => Packets.TryGetValue(packetID, out var packetConstructor) ? packetConstructor() : null;
        public override TPacketTypeCustom Create<TPacketTypeCustom>() => Packets.TryGetValue(IDTypeFromPacketType[typeof(TPacketTypeCustom)], out var packetConstructor) ? (TPacketTypeCustom)packetConstructor() : null;
        public override TPacketTypeCustom Create<TPacketTypeCustom>(Func<TPacketTypeCustom> initializer) => initializer();

        public override void Dispose() { }
    }
}