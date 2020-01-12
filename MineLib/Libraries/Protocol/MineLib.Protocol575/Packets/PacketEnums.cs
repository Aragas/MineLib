namespace MineLib.Protocol575.Packets
{
    public enum ClientLoginPacketTypes
    {
        Disconnect2                 = 0x00,
        EncryptionRequest           = 0x01,
        LoginSuccess                = 0x02,
    }
    public enum ClientPlayPacketTypes
    {

    }
    public enum ClientStatusPacketTypes
    {
        Response                    = 0x00,
        Ping                        = 0x01
    }

    public enum ServerLoginPacketTypes
    {
        LoginStart = 0x00,
        EncryptionResponse = 0x01
    }
    public enum ServerPlayPacketTypes
    {
        KeepAlive2 = 0x00,
        ChatMessage2 = 0x01,
        UseEntity = 0x02,
        Player = 0x03,
        PlayerPosition = 0x04,
        PlayerLook = 0x05,
        PlayerPositionAndLook2 = 0x06,
        PlayerDigging = 0x07,
        PlayerBlockPlacement = 0x08,
        HeldItemChange2 = 0x09,
        Animation2 = 0x0A,
        EntityAction = 0x0B,
        SteerVehicle = 0x0C,
        CloseWindow2 = 0x0D,
        ClickWindow = 0x0E,
        ConfirmTransaction2 = 0x0F,
        CreativeInventoryAction = 0x10,
        EnchantItem = 0x11,
        UpdateSign2 = 0x12,
        PlayerAbilities2 = 0x13,
        TabComplete2 = 0x14,
        ClientSettings = 0x15,
        ClientStatus = 0x16,
        PluginMessage2 = 0x17,
    }
    public enum ServerStatusPacketTypes
    {
        Request = 0x00,
        Ping2 = 0x01
    }
}