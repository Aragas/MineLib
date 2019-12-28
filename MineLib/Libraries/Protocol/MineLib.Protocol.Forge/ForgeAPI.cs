using MineLib.Protocol.Server;
using System;

namespace MineLib.Protocol.Forge
{
    public class ForgeAPI
    {
        public event EventHandler<ForgeEventArgs> OnMessage;

        public void HandlePluginMessage(string channel, ProtocolConnection client, byte[] data)
        {

        }
    }
}
