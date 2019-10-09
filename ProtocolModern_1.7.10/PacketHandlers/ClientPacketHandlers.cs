using System;

using Aragas.Core.Extensions;
using Aragas.Core.PacketHandlers;
using Aragas.Core.Packets;
using Aragas.Core.Wrappers;

using ProtocolModern.Enum;

namespace ProtocolModern.PacketHandlers
{
    internal static class ClientPacketHandlers
    {
        internal static class LoginPacketResponses
        {
            internal static readonly Func<IPacketHandlerContext, ContextFunc<ProtobufPacket>>[] Handlers;

            static LoginPacketResponses()
            {
                new ServerLoginPacketTypes().CreateHandlerInstancesOut(out Handlers, AppDomainWrapper.GetAssembly(typeof(LoginPacketResponses)));
            }
        }

        internal static class PlayPacketResponses
        {
            internal static readonly Func<IPacketHandlerContext, ContextFunc<ProtobufPacket>>[] Handlers;

            static PlayPacketResponses()
            {
                new ServerPlayPacketTypes().CreateHandlerInstancesOut(out Handlers, AppDomainWrapper.GetAssembly(typeof(PlayPacketResponses)));
            }
        }

        internal static class StatusPacketResponses
        {
            internal static readonly Func<IPacketHandlerContext, ContextFunc<ProtobufPacket>>[] Handlers;

            static StatusPacketResponses()
            {
                new ServerStatusPacketTypes().CreateHandlerInstancesOut(out Handlers, AppDomainWrapper.GetAssembly(typeof(StatusPacketResponses)));
            }
        }
    }
}
