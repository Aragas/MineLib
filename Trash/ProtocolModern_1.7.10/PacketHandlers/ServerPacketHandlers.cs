using System;

using Aragas.Core.Extensions;
using Aragas.Core.PacketHandlers;
using Aragas.Core.Packets;
using Aragas.Core.Wrappers;

using ProtocolModern.Enum;

namespace ProtocolModern.PacketHandlers
{
    internal static class ServerPacketHandlers
    {
        internal static class LoginPacketResponses
        {
            internal static readonly Func<IPacketHandlerContext, ContextFunc<ProtobufPacket>>[] Handlers;

            static LoginPacketResponses()
            {
                new ClientLoginPacketTypes().CreateHandlerInstancesOut(out Handlers, AppDomainWrapper.GetAssembly(typeof(LoginPacketResponses)));
            }
        }

        internal static class PlayPacketResponses
        {
            internal static readonly Func<IPacketHandlerContext, ContextFunc<ProtobufPacket>>[] Handlers;

            static PlayPacketResponses()
            {
                new ClientPlayPacketTypes().CreateHandlerInstancesOut(out Handlers, AppDomainWrapper.GetAssembly(typeof(PlayPacketResponses)));
            }
        }

        internal static class StatusPacketResponses
        {
            internal static readonly Func<IPacketHandlerContext, ContextFunc<ProtobufPacket>>[] Handlers;

            static StatusPacketResponses()
            {
                new ClientStatusPacketTypes().CreateHandlerInstancesOut(out Handlers, AppDomainWrapper.GetAssembly(typeof(StatusPacketResponses)));
            }
        }
    }
}
