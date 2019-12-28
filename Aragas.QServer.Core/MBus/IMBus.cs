using Aragas.TupleEventSystem;

using System;
using System.Threading.Tasks;

namespace Aragas.QServer.Core
{
    public interface IMBus : IDisposable
    {
        //event EventHandler<MBusMessageReceivedEventArgs> MessageReceived;
        BaseEventHandler<MBusMessageReceivedEventArgs> MessageReceived { get; set; }

        void SendMessage(in ReadOnlySpan<byte> message);
        Task SendMessageAsync(ReadOnlyMemory<byte> message);
    }
}