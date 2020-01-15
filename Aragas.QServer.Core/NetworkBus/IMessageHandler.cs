using System.Threading.Tasks;

namespace Aragas.QServer.Core.NetworkBus
{
    public interface IMessageReceiver<in TMessageRequest> where TMessageRequest : IMessage
    {
        Task HandleAsync(TMessageRequest message);
    }
    public interface IMessageHandler<in TMessageRequest, TMessageResponse> where TMessageRequest : IMessage where TMessageResponse : IMessage
    {
        Task<TMessageResponse> HandleAsync(TMessageRequest message);
    }
    public interface IMessageHandler<in TMessageRequest> where TMessageRequest : IMessage
    {
        Task<IMessage> HandleAsync(TMessageRequest message);
    }
}