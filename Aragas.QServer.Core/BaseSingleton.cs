using Aragas.QServer.Core.NetworkBus;

namespace Aragas.QServer.Core
{
    public class BaseSingleton
    {
        public static IAsyncNetworkBus Instance { get; } = new AsyncNATSBus();
        //public static IAsyncNetworkBus Instance { get; } = new AsyncSTANBus();
    }
}