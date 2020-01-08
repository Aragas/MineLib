
using System;

namespace Aragas.QServer.Core.NetworkBus
{
    public abstract class BaseNATSNetworkBus
    {
        protected static string GetSubject(IMessage message, Guid? referenceId = null)
        {
            var subject = message.Name;
            if (referenceId != null)
                subject += $"-{referenceId}";
            return subject.ToLowerInvariant();
        }
        protected static string GetSubject<TMessage>() where TMessage : notnull, IMessage, new() =>
            GetSubject(new TMessage());
        protected static string GetSubject<TMessage>(Guid? referenceId) where TMessage : notnull, IMessage, new() =>
            GetSubject(new TMessage(), referenceId);
    }
}