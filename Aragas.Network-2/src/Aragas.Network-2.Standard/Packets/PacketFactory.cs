using System;
using System.Collections.Generic;
using System.Linq;

using Aragas.Network.IO;

namespace Aragas.Network.Packets
{
    public abstract class BasePacketFactory<TPacketType, TIDType, TSerializer, TDeserializer> : IDisposable 
        where TPacketType : Packet<TIDType, TSerializer, TDeserializer>
        where TSerializer : PacketSerializer
        where TDeserializer : PacketDeserializer
    {
        public abstract TPacketType Create(TIDType packetID);
        public abstract TPacketTypeCustom Create<TPacketTypeCustom>() where TPacketTypeCustom : TPacketType;
        public abstract TPacketTypeCustom Create<TPacketTypeCustom>(Func<TPacketTypeCustom> initializer) where TPacketTypeCustom : new();

        public abstract void Dispose();
    }

    /*
    public class PacketEnumFactory<TPacketType, TEnum, TNumberType, TSerializer, TDeserializer> : BasePacketFactory<TPacketType, TNumberType, TSerializer, TDeserializer> 
        where TPacketType : PacketWithEnum<TEnum, TNumberType, TSerializer, TDeserializer>
        where TEnum : Enum
        where TSerializer : PacketSerializer 
        where TDeserializer : PacketDeserializer
    {
        private static Dictionary<Type, TNumberType> IDTypeFromPacketType { get; } = new Dictionary<Type, TNumberType>();
        private static Dictionary<TNumberType, Func<TPacketType>> Packets { get; } = new Dictionary<TNumberType, Func<TPacketType>>();
        private static bool IsInizialized { get; set; }
        private readonly object _lockObject = new object();

        public PacketEnumFactory()
        {
            if (!IsInizialized)
            {
                lock (_lockObject) // Thread safe
                {
                    if (!IsInizialized)
                    {
                        IsInizialized = true;

                        var whereToFindPackets = AppDomain.CurrentDomain.GetAssemblies();

                        foreach (var packetType in whereToFindPackets.Where(p => !p.IsDynamic).SelectMany(asm => asm.ExportedTypes.Where(type => type.IsSubclassOf(typeof(TPacketType)))))
                        {
                            var p = ActivatorCached.CreateInstance(packetType) as TPacketType;
                            Packets.Add(p.ID, packetType != null ? (Func<TPacketType>)(() => ActivatorCached.CreateInstance(packetType) as TPacketType) : null);
                            IDTypeFromPacketType.Add(p.GetType(), p.ID);
                        }
                    }
                }
            }
        }

        public override TPacketType Create(TNumberType packetID) => Packets.TryGetValue(packetID, out var packetConstructor) ? packetConstructor() : null;
        public override TPacketTypeCustom Create<TPacketTypeCustom>() => Packets.TryGetValue(IDTypeFromPacketType[typeof(TPacketTypeCustom)], out var packetConstructor) ? (TPacketTypeCustom) packetConstructor() : null;
        public override TPacketTypeCustom Create<TPacketTypeCustom>(Func<TPacketTypeCustom> initializer) => initializer();

        public override void Dispose() { }
    }

    public class PacketAttributeFactory<TPacketType, TIDType, TSerializer, TDeserializer> : BasePacketFactory<TPacketType, TIDType, TSerializer, TDeserializer>
        where TPacketType : PacketWithAttribute<TIDType, TSerializer, TDeserializer>
        where TIDType : struct
        where TSerializer : PacketSerializer
        where TDeserializer : PacketDeserializer
    {
        private static Dictionary<Type, TIDType> IDTypeFromPacketType { get; } = new Dictionary<Type, TIDType>();
        private static Dictionary<TIDType, Func<TPacketType>> Packets { get; } = new Dictionary<TIDType, Func<TPacketType>>();
        private static bool IsInizialized { get; set; }
        private readonly static object _lockObject = new object();

        public PacketAttributeFactory()
        {
            if (!IsInizialized)
            {
                lock (_lockObject) // Thread safe
                {
                    if (!IsInizialized)
                    {
                        IsInizialized = true;

                        var whereToFindPackets = AppDomain.CurrentDomain.GetAssemblies();
                        var packetTypes = whereToFindPackets.Where(p => !p.IsDynamic).SelectMany(asm => asm.ExportedTypes.Where(type => type.IsSubclassOf(typeof(TPacketType))));
                        var packetTypesWithAttribute = packetTypes.Where(type => type.IsDefined(typeof(PacketAttribute), false));
                        foreach (var packetType in packetTypesWithAttribute)
                        {
                            var p = ActivatorCached.CreateInstance(packetType) as PacketWithAttribute<TIDType, TSerializer, TDeserializer>;
                            Packets.Add(p.ID, () => (TPacketType) ActivatorCached.CreateInstance(packetType));
                            IDTypeFromPacketType.Add(p.GetType(), p.ID);
                        }
                    }
                }
            }
        }

        public override TPacketType Create(TIDType packetID) => Packets.TryGetValue(packetID, out var packetConstructor) ? packetConstructor() : null;
        public override TPacketTypeCustom Create<TPacketTypeCustom>() => Packets.TryGetValue(IDTypeFromPacketType[typeof(TPacketTypeCustom)], out var packetConstructor) ? (TPacketTypeCustom)packetConstructor() : null;
        public override TPacketTypeCustom Create<TPacketTypeCustom>(Func<TPacketTypeCustom> initializer) => initializer();

        public override void Dispose() { }
    }
    */

    public class DefaultPacketFactory<TPacketType, TIDType, TSerializer, TDeserializer> : BasePacketFactory<TPacketType, TIDType, TSerializer, TDeserializer> 
        where TPacketType : Packet<TIDType, TSerializer, TDeserializer>
        where TSerializer : PacketSerializer
        where TDeserializer : PacketDeserializer
    {
        private static readonly Dictionary<Type, TIDType> IDTypeFromPacketType = new Dictionary<Type, TIDType>();
        private static readonly Dictionary<TIDType, Func<TPacketType>> Packets = new Dictionary<TIDType, Func<TPacketType>>();
        private static readonly object _lockObject = new object();
        private static bool _isInizialized = false;

        public DefaultPacketFactory()
        {
            if (!_isInizialized)
            {
                lock (_lockObject)
                {
                    if (!_isInizialized)
                    {
                        _isInizialized = true;

                        var whereToFindPackets = AppDomain.CurrentDomain.GetAssemblies();
                        foreach (var packetType in whereToFindPackets.Where(p => !p.IsDynamic).SelectMany(asm => asm.ExportedTypes.Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(TPacketType)))))
                        {
                            var p = ActivatorCached.CreateInstance(packetType) as TPacketType; // -- We need to create a packet instance to get the ID
                            Packets.Add(p.ID, packetType != null ? (Func<TPacketType>)(() => ActivatorCached.CreateInstance(packetType) as TPacketType) : null);
                            IDTypeFromPacketType.Add(p.GetType(), p.ID);
                        }
                    }
                }
            }
        }

        public override TPacketType Create(TIDType packetID) => Packets.TryGetValue(packetID, out var packetConstructor) ? packetConstructor() : null;
        public override TPacketTypeCustom Create<TPacketTypeCustom>() => Packets.TryGetValue(IDTypeFromPacketType[typeof(TPacketTypeCustom)], out var packetConstructor) ? (TPacketTypeCustom) packetConstructor() : null;
        public override TPacketTypeCustom Create<TPacketTypeCustom>(Func<TPacketTypeCustom> initializer) => initializer();

        public override void Dispose() { }
    }
}