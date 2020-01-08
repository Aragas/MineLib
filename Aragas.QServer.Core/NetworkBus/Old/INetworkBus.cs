/*
using Aragas.QServer.Core.Extensions;

using NATS.Client;

using STAN.Client;

using System;
using System.Collections.Generic;

namespace Aragas.QServer.Core.NetworkBus
{
// Sub-Pub (Multiple listeners, one answer)
// Sub-Pub (Multiple listeners, all answer)

// Sub-AllPub                       (Broadcast)
// Sub-OnePub                       (Establish that only one pub will answer, notifying other pubs about it)
// Sub-AnyPub (Single Message)      (It's implied that only one pub should listen and answer)
// Sub-AnyPub (IEnumerable Message) (It's implied that only one pub should listen and answer)
public interface INetworkBus
{
    void Publish<TMessage>(TMessage message, Guid? referenceId = null)
        where TMessage : class, IMessage, new();
    TMessageResponse? PublishAndWaitForReply<TMessageRequest, TMessageResponse>(TMessageRequest message, Guid? referenceId = null, int timeout = -1)
        where TMessageRequest : class, IMessage, new()
        where TMessageResponse : class, IMessage, new();
    IEnumerable<TMessageResponse> PublishAndWaitForReplyEnumerable<TMessageRequest, TMessageResponse>(TMessageRequest message, Guid? referenceId = null, int timeout = -1)
        where TMessageRequest : class, IMessage, new()
        where TMessageResponse : class, IEnumerableMessage, new();
    //TMessageResponse? PublishAndWaitForOneReply<TMessageRequest, TMessageResponse>(TMessageRequest message, Guid? referenceId = null, int timeout = -1)
    //    where TMessageRequest : class, IMessage, new()
    //    where TMessageResponse : class, IEnumerableMessage, new();

    IDisposable Subscribe<TMessage>(Action<TMessage> func, Guid? referenceId = null)
        where TMessage : class, IMessage, new();
    IDisposable SubscribeAndReply<TMessageRequest>(Func<TMessageRequest, IMessage> func, Guid? referenceId = null)
        where TMessageRequest : class, IMessage, new();
    IDisposable SubscribeAndReplyEnumerable<TMessageRequest, TMessageResponse>(Func<TMessageRequest, IEnumerable<TMessageResponse>> func, Guid? referenceId = null)
        where TMessageRequest : class, IMessage, new()
        where TMessageResponse : class, IEnumerableMessage, new();

    IDisposable SubscribeAndReply<TMessageRequest>(IMessageHandler<TMessageRequest> handler, Guid? referenceId = null)
        where TMessageRequest : class, IMessage, new();
}

public sealed class NatsNetworkBus //: INetworkBus
{
    //public static INetworkBus Instance { get; } = new NatsNetworkBus();
    //public static IStanConnection Instance { get; } = new StanConnectionFactory().CreateConnection("test-cluster", Guid.NewGuid().ToString(), StanOptions.GetDefaultOptions().SetDefaultArgs());
    //public static IConnection Instance { get; } = new ConnectionFactory().CreateConnection(ConnectionFactory.GetDefaultOptions().SetDefaultArgs());

    public static int Timeout = 1500;

    private readonly IConnection _connection;

    public NatsNetworkBus() =>
        _connection = new ConnectionFactory().CreateConnection(ConnectionFactory.GetDefaultOptions().SetDefaultArgs());

    void INetworkBus.Publish<TMessage>(TMessage message, Guid? referenceId)
    {
        _connection.Publish(message, referenceId);
    }
    TMessageResponse INetworkBus.PublishAndWaitForReply<TMessageRequest, TMessageResponse>(TMessageRequest message, Guid? referenceId, int timeout)
    {
        var subject = new TMessageResponse().Name;
        if (referenceId != null)
            subject += $"-{referenceId}";

        using var sub = _connection.SubscribeSync(subject.ToLowerInvariant());
        _connection.Publish(message, referenceId);

        try
        {
            var responseMessage = sub.NextMessage(timeout);
            responseMessage.Respond(new byte[] { 0 });
            var response = new TMessageResponse();
            response.SetData(responseMessage.Data);
            return response;
        }
        catch (Exception e) when (e is NATSTimeoutException)
        {
            return null!;
        }
    }
    IEnumerable<TMessageResponse> INetworkBus.PublishAndWaitForReplyEnumerable<TMessageRequest, TMessageResponse>(TMessageRequest message, Guid? referenceId, int timeout)
    {
        var subject = new TMessageResponse().Name;
        if (referenceId != null)
            subject += $"-{referenceId}";

        using var sub = _connection.SubscribeSync(subject.ToLowerInvariant());
        _connection.Publish(message, referenceId);

        TMessageResponse lastMessage = default!;
        Msg? responseMessage = null;
        do
        {
            try
            {
                responseMessage = sub.NextMessage(timeout);
                lastMessage = new TMessageResponse();
                lastMessage.SetData(responseMessage.Data);
            }
            catch (Exception e) when (e is NATSTimeoutException)
            {
                yield break;
            }
            yield return lastMessage;
        } while (!lastMessage.IsLastMessage);
    }

    IDisposable INetworkBus.Subscribe<TMessage>(Action<TMessage> func, Guid? referenceId)
    {
        var subject = new TMessage().Name;
        if (referenceId != null)
            subject += $"-{referenceId}";

        return _connection.SubscribeAsync(subject.ToLowerInvariant(), (s, e) =>
        {
            var request = new TMessage();
            request.SetData(e.Message.Data);
            func(request);
        });
    }
    IDisposable INetworkBus.SubscribeAndReply<TMessageRequest>(Func<TMessageRequest, IMessage> func, Guid? referenceId)
    {
        var subject = new TMessageRequest().Name;
        if (referenceId != null)
            subject += $"-{referenceId}";

        return _connection.SubscribeAsync(subject.ToLowerInvariant(), (s, e) =>
        {
            var request = new TMessageRequest();
            request.SetData(e.Message.Data);
            var response = func(request);

            _connection.Publish(response, referenceId);
            //e.Message.Reply = response.Name;
            //e.Message.Respond(response.GetData());
        });
    }
    IDisposable INetworkBus.SubscribeAndReplyEnumerable<TMessageRequest, TMessageResponse>(Func<TMessageRequest, IEnumerable<TMessageResponse>> func, Guid? referenceId)
    {
        var subject = new TMessageRequest().Name;

        return _connection.SubscribeAsync(subject.ToLowerInvariant(), (s, e) =>
        {
            var request = new TMessageRequest();
            request.SetData(e.Message.Data);
            foreach (var response in func(request))
                _connection.Publish(response, referenceId);
        });
    }
    IDisposable INetworkBus.SubscribeAndReply<TMessageRequest>(IMessageHandler<TMessageRequest> handler, Guid? referenceId)
    {
        var subject = new TMessageRequest().Name;
        if (referenceId != null)
            subject += $"-{referenceId}";

        return _connection.SubscribeAsync(subject.ToLowerInvariant(), (s, e) =>
        {
            var request = new TMessageRequest();
            request.SetData(e.Message.Data);
            var response = handler.Handle(request);

            _connection.Publish(response, referenceId);
        });
    }
}
}
*/