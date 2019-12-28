using Aragas.TupleEventSystem;
using Aragas.QServer.Core.Packets;
using Aragas.QServer.Core.Packets.MBus;
using Aragas.QServer.Core.Protocol;

using System;

namespace Aragas.QServer.MBus
{
    internal sealed class MBusClient : InternalConnectionHandler
    {
        public BaseEventHandler<MBusClientMessageReceivedEventArgs> OnMessage { get; set; } = new WeakReferenceEventHandler<MBusClientMessageReceivedEventArgs>();
        //public event EventHandler<MBusClientMessageReceivedEventArgs>? OnMessage;

        public string? Name { get; set; }
        public Guid GUID { get; set; } = Guid.NewGuid();

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
                    OnMessage?.Invoke(this, new MBusClientMessageReceivedEventArgs(GUID, message.Data));
                    break;
            }
        }

        public void SendMessage(byte[] message) => SendPacket(new Message() { Data = message });

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                OnMessage?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}