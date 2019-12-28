using Aragas.QServer.Core;
using Aragas.QServer.Core.Protocol;

using System;
using System.Collections.Generic;

namespace Aragas.QServer.MBus
{
    internal sealed class MBusListener : InternalListener<MBusClient>
    {
        public override int Port { get; } = DefaultValues.MBus_Port;

        private List<MBusClient> Clients { get; } = new List<MBusClient>();

        protected override void OnClientConnected(MBusClient client)
        {
            client.StartListening();
            client.Disconnected += (this, Client_Disconnected);
            client.OnMessage += (this, Client_OnMessage);

            lock (Clients)
                Clients.Add(client);
        }

        private void Client_Disconnected(object? sender, EventArgs e)
        {
            if(sender is MBusClient client)
            {
                lock (Clients)
                    Clients.Remove(client);
                client.Dispose();
            }
        }

        private void Client_OnMessage(object? sender, MBusClientMessageReceivedEventArgs e)
        {
            lock(Clients)
            {
                foreach(var client in Clients)
                {
                    if (client.GUID != e.ClientGUID)
                        client.SendMessage(e.Message);
                }
            }
        }
    }
}