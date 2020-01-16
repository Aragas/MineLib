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

        public void Handle<TMessageHandler>(object[]? @params = null, Guid? referenceId = null)
            where TMessageHandler : notnull, IMessageHandler
        {
            if (@params == null)
                @params = Array.Empty<object>();

            dynamic handler = ActivatorUtilities.CreateInstance<TMessageHandler>(ServiceProvider, @params);
            RegisterHandler(handler, referenceId);
        }
        public void Receive<TMessageReceiver>(object[]? @params = null, Guid? referenceId = null)
            where TMessageReceiver : notnull, IMessageReceiver
        {
            if (@params == null)
                @params = Array.Empty<object>();

            dynamic handler = ActivatorUtilities.CreateInstance<TMessageReceiver>(ServiceProvider, @params);
            RegisterReceiver(handler, referenceId);
        }
        public void RegisterReceiver<TMessageRequest>(IMessageReceiver<TMessageRequest> receiver, Guid? referenceId = null)
            where TMessageRequest : IMessage, new()
        {
            Events.Add(NetworkBus.RegisterReceiver(receiver, referenceId));
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