using Microsoft.Extensions.DependencyInjection;

using System;
using System.Reactive.Disposables;

namespace Aragas.QServer.NetworkBus
{
    public sealed class SubscriptionStorage : IDisposable
    {
        private CompositeDisposable Events { get; } = new CompositeDisposable();

        private readonly IServiceProvider _serviceProvider;
        private readonly IAsyncNetworkBus _networkBus;

        public SubscriptionStorage(IServiceProvider serviceProvider, IAsyncNetworkBus networkBus)
        {
            _serviceProvider = serviceProvider;
            _networkBus = networkBus;
        }

        public void Handle<TMessageHandler, TMessageRequest>(Guid? referenceId = null, TMessageHandler? messageHandler = null)
            where TMessageHandler : class, IMessageHandler<TMessageRequest>
            where TMessageRequest : IMessage, new()
        {
            var handler = messageHandler ?? ActivatorUtilities.GetServiceOrCreateInstance<TMessageHandler>(_serviceProvider);
            RegisterHandler(handler, referenceId);
        }
        public void Handle<TMessageHandler, TMessageRequest, TMessageResponse>(Guid? referenceId = null, TMessageHandler? messageHandler = null)
            where TMessageHandler : class, IMessageHandler<TMessageRequest, TMessageResponse>
            where TMessageRequest : IMessage, new()
            where TMessageResponse : IMessage, new()
        {
            var handler = messageHandler ?? ActivatorUtilities.GetServiceOrCreateInstance<TMessageHandler>(_serviceProvider);
            RegisterHandler(handler, referenceId);
        }
        public void EnumerableHandle<TEnumerableMessageHandler, TMessageRequest, TMessageResponse>(Guid? referenceId = null, TEnumerableMessageHandler? messageHandler = null)
            where TEnumerableMessageHandler : class, IEnumerableMessageHandler<TMessageRequest, TMessageResponse>
            where TMessageRequest : IMessage, new()
            where TMessageResponse : IEnumerableMessage, new()
        {
            var handler = messageHandler ?? ActivatorUtilities.GetServiceOrCreateInstance<TEnumerableMessageHandler>(_serviceProvider);
            RegisterEnumerableHandler(handler, referenceId);
        }
        public void ExclusiveHandle<TExclusiveMessageHandler, TMessageRequest, TMessageResponse>(Guid referenceId, TExclusiveMessageHandler? messageHandler = null)
            where TExclusiveMessageHandler : class, IExclusiveMessageHandler<TMessageRequest, TMessageResponse>
            where TMessageRequest : IMessage, new()
            where TMessageResponse : IMessage, new()
        {
            var handler = messageHandler ?? ActivatorUtilities.GetServiceOrCreateInstance<TExclusiveMessageHandler>(_serviceProvider);
            RegisterExclusiveHandler(handler, referenceId);
        }
        public void Receive<TMessageReceiver, TMessageRequest>(Guid? referenceId = null, TMessageReceiver? messageReceiver = null)
            where TMessageReceiver : class, IMessageReceiver<TMessageRequest>
            where TMessageRequest : IMessage, new()
        {
            var receiver = messageReceiver ?? ActivatorUtilities.CreateInstance<TMessageReceiver>(_serviceProvider);
            RegisterReceiver(receiver, referenceId);
        }

        public void RegisterHandler<TMessageRequest, TMessageResponse>(IMessageHandler<TMessageRequest, TMessageResponse> handler, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IMessage, new()
        {
            Events.Add(_networkBus.RegisterHandler(handler, referenceId));
        }
        public void RegisterHandler<TMessageRequest>(IMessageHandler<TMessageRequest> handler, Guid? referenceId = null)
            where TMessageRequest : IMessage, new()
        {
            Events.Add(_networkBus.RegisterHandler(handler, referenceId));
        }
        public void RegisterEnumerableHandler<TMessageRequest, TMessageResponse>(IEnumerableMessageHandler<TMessageRequest, TMessageResponse> handler, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IEnumerableMessage, new()
        {
            Events.Add(_networkBus.RegisterEnumerableHandler(handler, referenceId));
        }
        public void RegisterExclusiveHandler<TMessageRequest, TMessageResponse>(IExclusiveMessageHandler<TMessageRequest, TMessageResponse> handler, Guid referenceId)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IMessage, new()
        {
            Events.Add(_networkBus.RegisterExclusiveHandler(handler, referenceId));
        }
        public void RegisterReceiver<TMessageRequest>(IMessageReceiver<TMessageRequest> receiver, Guid? referenceId = null)
            where TMessageRequest : IMessage, new()
        {
            Events.Add(_networkBus.RegisterReceiver(receiver, referenceId));
        }

        public void Dispose()
        {
            Events.Dispose();
        }
    }
}