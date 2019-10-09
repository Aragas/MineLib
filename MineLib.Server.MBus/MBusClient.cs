using MineLib.Server.Core.Packets;
using MineLib.Server.Core.Packets.MBus;
using MineLib.Server.Core.Protocol;

using System;

namespace MineLib.Server.MBus
{
    internal sealed class MBusClient : InternalConnectionHandler
    {
        public event EventHandler<MBusClientMessageReceivedEventArgs> OnMessage;

        public string Name { get; set; }
        public Guid GUID { get; set; } = Guid.NewGuid();

        public MBusClient() : base() { }

        protected override void HandlePacket(InternalPacket packet)
        {
            switch (packet)
            {
                case PingPacket pingPacket:
                    SendPacket(new PingPacket() { GUID = pingPacket.GUID });
                    break;

                case SubscribeRequest subscribeRequest:
                    Name = subscribeRequest.Name;
                    break;

                case Message message:
                    OnMessage?.Invoke(this, new MBusClientMessageReceivedEventArgs() { ClientGUID = GUID, Message = message.Data });
                    break;
            }
        }

        public void SendMessage(byte[] message)
        {
            SendPacket(new Message() { Data = message });
        }
    }
}
