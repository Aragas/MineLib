using System.Linq;

using Aragas.Network.Data;

using MineLib.Protocol.Packets;

namespace MineLib.Protocol.Netty.Packets
{
    public abstract class ProtocolNettyPacket<TEnum> : MinecraftEnumPacket where TEnum : System.Enum
    {
        private static TEnum[] Cache { get; } = System.Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();

        public override VarInt ID { get; }

        protected ProtocolNettyPacket() => ID = (VarInt) (dynamic) Cache.Single(@enum => GetType().Name == $"{@enum}Packet");
    }
}