using Aragas.QServer.Core.Extensions;
using Aragas.TupleEventSystem;

using NATS.Client;

using System;
using System.Threading.Tasks;

namespace Aragas.QServer.Core.MBus
{
    public sealed class NatsMBus : IMBus
    {
        public BaseEventHandler<MBusMessageReceivedEventArgs> MessageReceived { get; set; } = new WeakReferenceEventHandler<MBusMessageReceivedEventArgs>();

        private string Name { get; }
        private IConnection Connection { get; }
        private IAsyncSubscription Subscription { get; }

        public NatsMBus(string name, TimeSpan? messageTTL = null)
        {
            Name = name;
            Connection = new ConnectionFactory().CreateConnection(ConnectionFactory.GetDefaultOptions().SetDefaultArgs(messageTTL));
            Subscription = Connection.SubscribeAsync(name, (s, e) => MessageReceived?.Invoke(this, new MBusMessageReceivedEventArgs(e.Message.Data)));
        }

        public void SendMessage(in ReadOnlySpan<byte> message) =>
            Connection.Publish(Name, message.ToArray());

        public Task SendMessageAsync(ReadOnlyMemory<byte> message) => Task.Run(() =>
            Connection.Publish(Name, message.ToArray()));

        public void Dispose()
        {
            Subscription?.Dispose();
            MessageReceived?.Dispose();
            Connection?.Dispose();
        }
    }
}