using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aragas.QServer.NetworkBus
{
    public interface INetworkBus : IDisposable
    {
        void Publish<TMessage>(TMessage message, Guid? referenceId = null)
            where TMessage : notnull, IMessage;
        TMessageResponse? PublishAndWaitForReply<TMessageRequest, TMessageResponse>(TMessageRequest message, Guid? referenceId = null, int timeout = -1)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : class, IMessage, new();
        IEnumerable<TMessageResponse> PublishAndWaitForReplyEnumerable<TMessageRequest, TMessageResponse>(TMessageRequest message, Guid? referenceId = null, int timeout = -1)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IEnumerableMessage, new();

        IDisposable Subscribe<TMessage>(Action<TMessage> func, Guid? referenceId = null)
            where TMessage : notnull, IMessage, new();
        IDisposable SubscribeAndReply<TMessageRequest>(Func<TMessageRequest, IMessage> func, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new();
        IDisposable SubscribeAndReplyEnumerable<TMessageRequest, TMessageResponse>(Func<TMessageRequest, IEnumerable<TMessageResponse>> func, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IEnumerableMessage, new();
    }

    public interface IAsyncNetworkBus : INetworkBus
    {
        Task PublishAsync<TMessage>(TMessage message, Guid? referenceId = null)
            where TMessage : notnull, IMessage;
        Task<TMessageResponse?> PublishAndWaitForReplyAsync<TMessageRequest, TMessageResponse>(TMessageRequest message, Guid? referenceId = null, int timeout = -1)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : class, IMessage, new();
        IAsyncEnumerable<TMessageResponse> PublishAndWaitForReplyEnumerableAsync<TMessageRequest, TMessageResponse>(TMessageRequest message, Guid? referenceId = null, int timeout = -1)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IEnumerableMessage, new();

        Task<IDisposable> SubscribeAsync<TMessage>(Func<TMessage, Task> func, Guid? referenceId = null)
            where TMessage : notnull, IMessage, new();
        Task<IDisposable> SubscribeAndReplyAsync<TMessageRequest>(Func<TMessageRequest, Task<IMessage>> func, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new();
        Task<IDisposable> SubscribeAndReplyEnumerableAsync<TMessageRequest, TMessageResponse>(Func<TMessageRequest, IAsyncEnumerable<TMessageResponse>> func, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IEnumerableMessage, new();
    }
}