using System;
using System.Threading.Tasks;

namespace MineLib.Server.Core
{
    public interface IMBus : IDisposable
    {
        event EventHandler<MBusMessageReceivedEventArgs> MessageReceived;

        void SendMessage(in ReadOnlySpan<byte> message);
        Task SendMessageAsync(ReadOnlyMemory<byte> message);
    }
}