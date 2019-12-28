using Aragas.TupleEventSystem;

using System;
using System.Threading.Tasks;

using NATS.Client;

namespace Aragas.QServer.Core
{
    public sealed class NatsMBus : IMBus
    {
        public BaseEventHandler<MBusMessageReceivedEventArgs> MessageReceived { get; set; } = new WeakReferenceEventHandler<MBusMessageReceivedEventArgs>();

        private string BusName { get; }
        private IConnection Connection { get; set; }
        private IAsyncSubscription Subscription { get; }

        public NatsMBus(string busName, TimeSpan? messageTTL = null)
        {
            BusName = busName;

            var opts = ConnectionFactory.GetDefaultOptions();
            opts.Timeout = (int) messageTTL.GetValueOrDefault(TimeSpan.FromMilliseconds(10000)).TotalMilliseconds;
            opts.Url = "minelib.nats:4222";
            Connection = new ConnectionFactory().CreateConnection(opts);
            Subscription = Connection.SubscribeAsync(BusName, (s, e) => MessageReceived?.Invoke(this, new MBusMessageReceivedEventArgs(e.Message.Data)));
        }

        public void SendMessage(in ReadOnlySpan<byte> message)
        {
            Connection.Publish(BusName, message.ToArray());
        }

        public Task SendMessageAsync(ReadOnlyMemory<byte> message)
        {
            Connection.Publish(BusName, message.ToArray());
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Subscription.Dispose();
            MessageReceived.Dispose();
            Connection.Dispose();
        }
    }
}