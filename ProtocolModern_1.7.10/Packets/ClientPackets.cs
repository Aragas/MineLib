using System;

using Aragas.Core.Extensions;
using Aragas.Core.Packets;
using Aragas.Core.Wrappers;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets
{
    public static class ClientPackets
    {
        public static class LoginPacketResponses
        {
            public static readonly Func<ProtobufPacket>[] Packets;

            static LoginPacketResponses()
            {
                new ClientLoginPacketTypes().CreatePacketInstancesOut(out Packets, AppDomainWrapper.GetAssembly(typeof(LoginPacketResponses)));
            }
        }

        public static class PlayPacketResponses
        {
            public static readonly Func<ProtobufPacket>[] Packets;

            static PlayPacketResponses()
            {
                new ClientPlayPacketTypes().CreatePacketInstancesOut(out Packets, AppDomainWrapper.GetAssembly(typeof(PlayPacketResponses)));
            }
        }

        public static class StatusPacketResponses
        {
            public static readonly Func<ProtobufPacket>[] Packets;

            static StatusPacketResponses()
            {
                new ClientStatusPacketTypes().CreatePacketInstancesOut(out Packets, AppDomainWrapper.GetAssembly(typeof(StatusPacketResponses)));
            }
        }
    }
}
