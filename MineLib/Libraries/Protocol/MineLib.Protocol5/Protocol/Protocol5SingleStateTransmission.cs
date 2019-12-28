using System;
using System.Net.Sockets;

using MineLib.Protocol.Protocol;
using MineLib.Protocol5.Packets;

namespace MineLib.Protocol5.Protocol
{
    /// <summary>
    /// Handles a single State, for example, Play State.
    /// </summary>
    public class Protocol5SingleStateTransmission<TPacketType, TEnum> : MinecraftSingleStateTransmission<TPacketType>
        where TPacketType : Protocol5Packet<TEnum>
        where TEnum : Enum
    {
        /*
        public Protocol5SingleStateTransmission(Socket socket) : base(socket, new Protocol5Factory<TPacketType, TEnum>()) { }
        public Protocol5SingleStateTransmission(Socket socket, Protocol5Factory<TPacketType, TEnum> factory) : base(socket, factory) { }
        */
    }
}