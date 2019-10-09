using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MineLib.IPC
{
    public class BaseIPC
    {
        public ushort Port { get; protected set; }

        public BaseIPC(string name) : this(".", name) { }
        public BaseIPC(string server, string name)
        {

        }

        /*
        public void SendData(byte[] data)
        {

        }

        public byte[] ReceiveData()
        {
            return null;
        }
        */
    }

    public class IPCBroadcaster : BaseIPC
    {
        protected TcpListener Listener { get; }
        protected List<TcpClient> Clients { get; } = new List<TcpClient>();

        public IPCBroadcaster(string name) : this(".", name) { }
        public IPCBroadcaster(string server, string name) : base(server, name)
        {
            Listener = new TcpListener(IPAddress.Any, Port);
            Listener.Server.ReceiveTimeout = 5000;
            Listener.Server.SendTimeout = 5000;
            Listener.Start();
            Console.WriteLine($"Started {GetType().Name} on port {Port}.");

            new Thread(Cycle)
            {
                Name = $"{GetType().Name}Thread",
                IsBackground = true
            }.Start();
            Console.WriteLine($"Started {GetType().Name}Thread.");
        }

        private void Cycle()
        {
            try
            {
                while (true) // Listener.Stop() will stop it.
                {
                    var client = Listener.AcceptTcpClient();

                    lock (Clients)
                        Clients.Add(client);

#if DEBUG
                    Console.WriteLine($"{client.GetType().Name} connected.");
#endif
                }

            }
            catch (SocketException) { }
        }

        public void SendData(byte[] data)
        {
            lock (Clients)
                for (var i = 0; i < Clients.Count; i++)
                    Clients[i].Client.Send(data);
        }

        public void ReceiveData()
        {

        }
    }

    public class IPCListener
    {

    }

    // We need a library that should have Broadcaster and Listener classes
    // Or something like a DBus, but we have there a centralized process o handle data
    // Or create a class that uses NamedPipes for finding the endpoint without the IP's over
    // local network, then use something 

    // Basically, we have a Broadcaster that can be found via IP endoint, where Listeners
    // subscribe to it and wait for messages

    // We should have one Machine with a static IP that everyone knows, where every program
    // will send their IP endpoint and their function
    public class Class1
    {
    }
}
