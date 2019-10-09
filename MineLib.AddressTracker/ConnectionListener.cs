using MineLib.Server.Core;

namespace MineLib.AddressTracker
{
    public class ConnectionListener : StandardInternalListener<Connection>
    {
        public override int Port { get; } = DefaultValues.AddressTracker_Port;
    }
}