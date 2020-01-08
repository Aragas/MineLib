namespace Aragas.QServer.Core.NetworkBus.Messages
{
    public sealed class PlayerDataToBusMessage : BinaryMessage
    {
        public override string Name => "services.playerbus.player.data.tobus";
    }
    public sealed class PlayerDataToProxyMessage : BinaryMessage
    {
        public override string Name => "services.playerbus.player.data.toproxy";
    }
}