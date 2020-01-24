using Aragas.QServer.Core.NetworkBus;

using System;

namespace Aragas.QServer.Core
{
    [Obsolete]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1102:Make class static.", Justification = "<Pending>")]
    public class BaseSingleton
    {
        public static IAsyncNetworkBus Instance { get; } = new AsyncNATSBus();
        //public static IAsyncNetworkBus Instance { get; } = new AsyncSTANBus();
    }
}