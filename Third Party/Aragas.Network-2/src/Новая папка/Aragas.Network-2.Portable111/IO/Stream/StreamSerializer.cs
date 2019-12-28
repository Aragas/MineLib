namespace Aragas.Network.IO
{
    public abstract class StreamSerializer : PacketSerializer
    {
        public abstract byte[] GetData();
    }
}