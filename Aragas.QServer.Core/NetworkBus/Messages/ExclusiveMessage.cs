using System;

namespace Aragas.QServer.Core.NetworkBus.Messages
{
    public class ExclusiveRequestMessage<TMessage> : IMessage where TMessage : IMessage, new()
    {
        public string Name { get; } = $"services.exclusive.request-[{new TMessage().Name}]";

        public TMessage Request { get; }

        public ExclusiveRequestMessage() => Request = new TMessage();
        public ExclusiveRequestMessage(TMessage request) => Request = request;

        public ReadOnlySpan<byte> GetData() => Request.GetData();
        public void SetData(in ReadOnlySpan<byte> data) => Request.SetData(in data);
    }
    public class ExclusiveResponseMessage<TMessage> : IMessage where TMessage : IMessage, new()
    {
        public string Name { get; } = $"services.exclusive.response-[{new TMessage().Name}]";

        public Guid ReferenceId { get; private set; }

        public ExclusiveResponseMessage() { }
        public ExclusiveResponseMessage(Guid referenceId) => ReferenceId = referenceId;

        public ReadOnlySpan<byte> GetData() => ReferenceId.ToByteArray();
        public void SetData(in ReadOnlySpan<byte> data) => ReferenceId = new Guid(data);
    }

    public class ExclusiveAcceptedRequestMessage<TMessage> : IMessage where TMessage : IMessage, new()
    {
        public string Name => $"services.exclusive.accepted.request-[{Request.Name}]";

        public TMessage Request { get; }

        public ExclusiveAcceptedRequestMessage() => Request = new TMessage();
        public ExclusiveAcceptedRequestMessage(TMessage request) => Request = request;

        public ReadOnlySpan<byte> GetData() => Request.GetData();
        public void SetData(in ReadOnlySpan<byte> data) => Request.SetData(in data);
    }
    public class ExclusiveAcceptedResponseMessage<TMessage> : IMessage where TMessage : IMessage, new()
    {
        public string Name => $"services.exclusive.accepted.response-[{Response.Name}]";

        public TMessage Response { get; }

        public ExclusiveAcceptedResponseMessage() => Response = new TMessage();
        public ExclusiveAcceptedResponseMessage(TMessage response) => Response = response;

        public ReadOnlySpan<byte> GetData() => Response.GetData();
        public void SetData(in ReadOnlySpan<byte> data) => Response.SetData(in data);
    }
}