/*
using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.NetworkBus;

using NATS.Client;

using System;
using System.Collections.Generic;
using System.Threading;

namespace Aragas.QServer.Core.MBus.Channels
{
    public interface INetworkBusV1
    {
        void BroadNATSProtobufTransmission<TMessage>(TMessage message)
            where TMessage : IMessage;
        void BroadcastInstance<TMessage>(Guid serviceId, TMessage message)
            where TMessage : IMessage;

        TMessageResponse? Send<TMessageRequest, TMessageResponse>(TMessageRequest message)
            where TMessageRequest : IMessage
            where TMessageResponse : class, IMessage, new();
        TMessageResponse? SendInstance<TMessageRequest, TMessageResponse>(Guid serviceId, TMessageRequest message)
            where TMessageRequest : IMessage
            where TMessageResponse : class, IMessage, new();
        IEnumerable<TMessageResponse> Send<TMessageRequest, TMessageResponse>(TMessageRequest message, CancellationToken cancellationToken)
            where TMessageRequest : IMessage
            where TMessageResponse : IMessage, new();

        IDisposable Listen<TMessage>(Action<TMessage> func)
            where TMessage : IMessage, new();
        IDisposable ListenInstance<TMessage>(Guid serviceId, Action<TMessage> func)
            where TMessage : IMessage, new();

        IDisposable ListenAndReply<TMessageRequest>(IMessageHandler<TMessageRequest> handler)
            where TMessageRequest : IMessage, new();
        IDisposable ListenAndReply<TMessageRequest>(Func<TMessageRequest, IMessage> func)
            where TMessageRequest : IMessage, new();
        IDisposable ListenAndReply<TMessageRequest>(Func<TMessageRequest, IEnumerable<IMessage>> func)
            where TMessageRequest : IMessage, new();
    }
    public sealed class NatsNetworkBusV1 : INetworkBusV1
    {
        public static INetworkBusV1 Instance { get; } = new NatsNetworkBusV1();
        public static int Timeout = 1500;

        private readonly IConnection _connection;

        public NatsNetworkBusV1()
        {
            _connection = new ConnectionFactory().CreateConnection(ConnectionFactory.GetDefaultOptions().SetDefaultArgs());
        }

        public void Broadcast<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            _connection.Publish(message);
            _connection.Flush();
        }
        public void BroadcastInstance<TMessage>(Guid serviceId, TMessage message)
            where TMessage : IMessage
        {
            _connection.Publish($"{message.Name}-{serviceId}".ToLowerInvariant(), message.GetData());
            _connection.Flush();
        }

        public TMessageResponse? Send<TMessageRequest, TMessageResponse>(TMessageRequest message)
            where TMessageRequest : IMessage
            where TMessageResponse : class, IMessage, new()
        {
            using var sub = _connection.SubscribeSync(new TMessageResponse().Name.ToLowerInvariant());
            _connection.Publish(message);
            _connection.Flush();
            var response = new TMessageResponse();
            Msg? responseMessage = null;
            try { responseMessage = sub.NextMessage(Timeout); }
            catch (Exception e) when (e is NATSTimeoutException) { }
            sub.Unsubscribe();
            response.SetData(responseMessage.Data);
            return response;

            //var response = new TMessageResponse();
            //Msg? responseMessage = null;
            //try { responseMessage = _connection.Request(message, Timeout); }
            //catch (Exception e) when (e is NATSTimeoutException) { }
            //if (responseMessage == null)
            //    return null;
            //response.SetData(responseMessage.Data);
            //return response;
        }
        public TMessageResponse? SendInstance<TMessageRequest, TMessageResponse>(Guid serviceId, TMessageRequest message)
            where TMessageRequest : IMessage
            where TMessageResponse : class, IMessage, new()
        {
            using var sub = _connection.SubscribeSync($"{new TMessageResponse().Name}-{serviceId}".ToLowerInvariant());
            _connection.Publish(message, serviceId);
            _connection.Flush();
            var response = new TMessageResponse();
            Msg? responseMessage = null;
            try { responseMessage = sub.NextMessage(Timeout); }
            catch (Exception e) when (e is NATSTimeoutException) { }
            sub.Unsubscribe();
            response.SetData(responseMessage.Data);
            return response;
        }
        public IEnumerable<TMessageResponse> Send<TMessageRequest, TMessageResponse>(TMessageRequest message, CancellationToken cancellationToken)
            where TMessageRequest : IMessage
            where TMessageResponse : IMessage, new()
        {
            using var sub = _connection.SubscribeSync(new TMessageResponse().Name.ToLowerInvariant());
            _connection.Publish(message);
            _connection.Flush();
            while (!cancellationToken.IsCancellationRequested)
            {
                var resp = new TMessageResponse();
                Msg? responseMessage = null;
                try { responseMessage = sub.NextMessage(Timeout); }
                catch (Exception e) when (e is NATSTimeoutException) { }
                if (responseMessage == null) continue; //yield break;
                resp.SetData(responseMessage.Data);
                yield return resp;
            }
        }

        public IDisposable ListenAndReply<TMessageRequest>(IMessageHandler<TMessageRequest> handler)
            where TMessageRequest : IMessage, new()
        {
            return _connection.SubscribeAsync(new TMessageRequest().Name.ToLowerInvariant(), (s, e) =>
            {
                var request = new TMessageRequest();
                request.SetData(e.Message.Data);
                var response = handler.Handle(request);
                if (response != null)
                {
                    e.Message.Reply = response.Name;
                    e.Message.Respond(response.GetData());
                }
            });
        }
        public IDisposable ListenAndReply<TMessageRequest>(IMessageHandlerService<TMessageRequest> handler)
            where TMessageRequest : IMessage, new()
        {
            return _connection.SubscribeAsync($"{new TMessageRequest().Name}-{handler.ServiceId}".ToLowerInvariant(), (s, e) =>
            {
                var request = new TMessageRequest();
                request.SetData(e.Message.Data);
                var response = handler.Handle(request);
                if (response != null)
                {
                    e.Message.Reply = response.Name;
                    e.Message.Respond(response.GetData());
                }
            });
        }
        public IDisposable ListenAndReply<TMessageRequest>(Func<TMessageRequest, IMessage> func)
            where TMessageRequest : IMessage, new()
        {
            return _connection.SubscribeAsync(new TMessageRequest().Name.ToLowerInvariant(), (s, e) =>
            {
                var request = new TMessageRequest();
                request.SetData(e.Message.Data);
                var response = func(request);
                e.Message.Reply = response.Name;
                e.Message.Respond(response.GetData());
            });
        }
        public IDisposable ListenAndReply<TMessageRequest>(Func<TMessageRequest, IEnumerable<IMessage>> func)
            where TMessageRequest : IMessage, new()
        {
            return _connection.SubscribeAsync(new TMessageRequest().Name.ToLowerInvariant(), (s, e) =>
            {
                var request = new TMessageRequest();
                request.SetData(e.Message.Data);
                foreach (var response in func(request))
                {
                    e.Message.Reply = response.Name;
                    e.Message.Respond(response.GetData());
                }
            });
        }

        public IDisposable Listen<TMessage>(Action<TMessage> func) where TMessage : IMessage, new()
        {
            return _connection.SubscribeAsync(new TMessage().Name.ToLowerInvariant(), (s, e) =>
            {
                var request = new TMessage();
                request.SetData(e.Message.Data);
                func(request);
            });
        }
        public IDisposable ListenInstance<TMessage>(Guid serviceId, Action<TMessage> func) where TMessage : IMessage, new()
        {
            return _connection.SubscribeAsync($"{new TMessage().Name}-{serviceId}".ToLowerInvariant(), (s, e) =>
            {
                var request = new TMessage();
                request.SetData(e.Message.Data);
                func(request);
            });
        }
    }
}
*/