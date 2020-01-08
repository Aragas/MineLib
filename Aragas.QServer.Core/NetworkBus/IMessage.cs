using System;

namespace Aragas.QServer.Core.NetworkBus
{
    // Send message request implying only one can answer
    // Everyone sends message reply
    // Pick one and send accepted message response
    // Send everyone else denied message response
    public interface IMessage
    {
        string Name { get; }

        ReadOnlySpan<byte> GetData();
        void SetData(in ReadOnlySpan<byte> data);
    }

    //public interface IMessageRequest : IMessage { }
    //public interface IMessageResponse : IMessage { }

    public interface IEnumerableMessage : IMessage
    {
        bool IsLastMessage { get; }
    }
}