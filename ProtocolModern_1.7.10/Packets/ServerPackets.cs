using System;

using Aragas.Core.Extensions;
using Aragas.Core.Packets;
using Aragas.Core.Wrappers;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets
{
    public static class ServerPackets
    {
        public static class HandshakePacketResponses
        {
            public static readonly Func<ProtobufPacket>[] Packets;

            static HandshakePacketResponses()
            {
                new ServerHandshakePacketTypes().CreatePacketInstancesOut(out Packets, AppDomainWrapper.GetAssembly(typeof(HandshakePacketResponses)));
            }
        }

        public static class LoginPacketResponses
        {
            public static readonly Func<ProtobufPacket>[] Packets;

            static LoginPacketResponses()
            {
                new ServerLoginPacketTypes().CreatePacketInstancesOut(out Packets, AppDomainWrapper.GetAssembly(typeof(LoginPacketResponses)));
            }
        }

        public static class PlayPacketResponses
        {
            public static readonly Func<ProtobufPacket>[] Packets;

            static PlayPacketResponses()
            {
                new ServerPlayPacketTypes().CreatePacketInstancesOut(out Packets, AppDomainWrapper.GetAssembly(typeof(PlayPacketResponses)));
            }
        }

        public static class StatusPacketResponses
        {
            public static readonly Func<ProtobufPacket>[] Packets;

            static StatusPacketResponses()
            {
                new ServerStatusPacketTypes().CreatePacketInstancesOut(out Packets, AppDomainWrapper.GetAssembly(typeof(StatusPacketResponses)));
            }
        }
    }
}
