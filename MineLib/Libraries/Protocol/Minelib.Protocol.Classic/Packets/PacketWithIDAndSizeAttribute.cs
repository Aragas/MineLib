using Aragas.Network.Packets;

using System.Reflection;

namespace MineLib.Protocol.Classic.Packets
{
    public abstract class PacketWithIDAndSizeAttribute<TIDType> : PacketWithIDAttribute<TIDType>
    {
        private Initializable<int> _size;
        public int Size
        {
            get
            {
                var size = _size.HasInitialized ? _size : (_size = GetType().GetCustomAttribute<PacketSizeAttribute>().Size);
                return size.Value;
            }
        }
    }
}