using System.Threading.Tasks;

namespace Aragas.QServer.Core.NetworkBus
{
    public interface IMessageReceiver { }
    public interface IMessageReceiver<in TMessageRequest> : IMessageReceiver
        where TMessageRequest : IMessage
    {
        Task HandleAsync(TMessageRequest message);
    }

    public interface IMessageHandler { }
    public interface IMessageHandler<in TMessageRequest, TMessageResponse> : IMessageHandler
        where TMessageRequest : IMessage
        where TMessageResponse : IMessage
    {
        Task<TMessageResponse> HandleAsync(TMessageRequest message);
    }
    public interface IMessageHandler<in TMessageRequest> : IMessageHandler
        where TMessageRequest : IMessage
    {
        Task<IMessage> HandleAsync(TMessageRequest message);
    }
}