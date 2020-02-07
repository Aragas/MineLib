using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aragas.QServer.NetworkBus
{
    public interface IMessageReceiver<in TMessageRequest>
        where TMessageRequest : IMessage
    {
        Task HandleAsync(TMessageRequest message);
    }

    public interface IMessageHandler<in TMessageRequest, TMessageResponse>
        where TMessageRequest : IMessage
        where TMessageResponse : IMessage
    {
        Task<TMessageResponse> HandleAsync(TMessageRequest message);
    }
    public interface IMessageHandler<in TMessageRequest>
        where TMessageRequest : IMessage
    {
        Task<IMessage> HandleAsync(TMessageRequest message);
    }

    public interface IEnumerableMessageHandler<in TMessageRequest, out TMessageResponse>
        where TMessageRequest : IMessage
        where TMessageResponse : IEnumerableMessage
    {
        IAsyncEnumerable<TMessageResponse> HandleAsync(TMessageRequest message);
    }

    public interface IExclusiveMessageHandler<in TMessageRequest, TMessageResponse>
        where TMessageRequest : IMessage
        where TMessageResponse : IMessage
    {
        Task<bool> CanHandle(TMessageRequest message);
        Task<TMessageResponse> HandleAsync(TMessageRequest message);
    }
}