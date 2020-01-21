using Aragas.QServer.Core.Extensions;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Reactive.Disposables;

namespace Aragas.QServer.Core.NetworkBus
{
    public class SubscriptionStorage : IDisposable
    {
        protected CompositeDisposable Events { get; } = new CompositeDisposable();
        protected IAsyncNetworkBus NetworkBus { get; }
        protected IServiceProvider ServiceProvider { get; }

        public SubscriptionStorage(IServiceProvider serviceProvider, IAsyncNetworkBus networkBus)
        {
            ServiceProvider = serviceProvider;
            NetworkBus = networkBus;
        }

        public void Handle<TMessageHandler, TMessageRequest>(Guid? referenceId = null, TMessageHandler? messageHandler = null)
            where TMessageHandler : class, IMessageHandler<TMessageRequest>
            where TMessageRequest : IMessage, new()
        {
            var handler = messageHandler ?? ActivatorUtilities.GetServiceOrCreateInstance<TMessageHandler>(ServiceProvider);
            RegisterHandler(handler, referenceId);
        }
        public void Handle<TMessageHandler, TMessageRequest, TMessageResponse>(Guid? referenceId = null, TMessageHandler? messageHandler = null)
            where TMessageHandler : class, IMessageHandler<TMessageRequest, TMessageResponse>
            where TMessageRequest : IMessage, new()
            where TMessageResponse : IMessage, new()
        {
            var handler = messageHandler ?? ActivatorUtilities.GetServiceOrCreateInstance<TMessageHandler>(ServiceProvider);
            RegisterHandler(handler, referenceId);
        }
        public void EnumerableHandle<TEnumerableMessageHandler, TMessageRequest, TMessageResponse>(Guid? referenceId = null, TEnumerableMessageHandler? messageHandler = null)
            where TEnumerableMessageHandler : class, IEnumerableMessageHandler<TMessageRequest, TMessageResponse>
            where TMessageRequest : IMessage, new()
            where TMessageResponse : IEnumerableMessage, new()
        {
            var handler = messageHandler ?? ActivatorUtilities.GetServiceOrCreateInstance<TEnumerableMessageHandler>(ServiceProvider);
            RegisterEnumerableHandler(handler, referenceId);
        }
        public void ExclusiveHandle<TExclusiveMessageHandler, TMessageRequest, TMessageResponse>(Guid referenceId, TExclusiveMessageHandler? messageHandler = null)
            where TExclusiveMessageHandler : class, IExclusiveMessageHandler<TMessageRequest, TMessageResponse>
            where TMessageRequest : IMessage, new()
            where TMessageResponse : IMessage, new()
        {
            var handler = messageHandler ?? ActivatorUtilities.GetServiceOrCreateInstance<TExclusiveMessageHandler>(ServiceProvider);
            RegisterExclusiveHandler(handler, referenceId);
        }
        public void Receive<TMessageReceiver, TMessageRequest>(Guid? referenceId = null, TMessageReceiver? messageReceiver = null)
            where TMessageReceiver : class, IMessageReceiver<TMessageRequest>
            where TMessageRequest : IMessage, new()
        {
            var receiver = messageReceiver ?? ActivatorUtilities.CreateInstance<TMessageReceiver>(ServiceProvider);
            RegisterReceiver(receiver, referenceId);
        }

        public void RegisterHandler<TMessageRequest, TMessageResponse>(IMessageHandler<TMessageRequest, TMessageResponse> handler, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IMessage, new()
        {
            Events.Add(NetworkBus.RegisterHandler(handler, referenceId));
        }
        public void RegisterHandler<TMessageRequest>(IMessageHandler<TMessageRequest> handler, Guid? referenceId = null)
            where TMessageRequest : IMessage, new()
        {
            Events.Add(NetworkBus.RegisterHandler(handler, referenceId));
        }
        public void RegisterEnumerableHandler<TMessageRequest, TMessageResponse>(IEnumerableMessageHandler<TMessageRequest, TMessageResponse> handler, Guid? referenceId = null)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IEnumerableMessage, new()
        {
            Events.Add(NetworkBus.RegisterEnumerableHandler(handler, referenceId));
        }
        public void RegisterExclusiveHandler<TMessageRequest, TMessageResponse>(IExclusiveMessageHandler<TMessageRequest, TMessageResponse> handler, Guid referenceId)
            where TMessageRequest : notnull, IMessage, new()
            where TMessageResponse : notnull, IMessage, new()
        {
            Events.Add(NetworkBus.RegisterExclusiveHandler(handler, referenceId));
        }
        public void RegisterReceiver<TMessageRequest>(IMessageReceiver<TMessageRequest> receiver, Guid? referenceId = null)
            where TMessageRequest : IMessage, new()
        {
            Events.Add(NetworkBus.RegisterReceiver(receiver, referenceId));
        }

        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Events.Dispose();
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}