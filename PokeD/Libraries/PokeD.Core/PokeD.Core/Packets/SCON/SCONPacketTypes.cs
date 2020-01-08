namespace PokeD.Core.Packets.SCON
{
    public enum SCONPacketTypes
    {
        AuthorizationRequest        = 0x00,
        AuthorizationResponse       = 0x01,

        EncryptionRequest           = 0x02,
        EncryptionResponse          = 0x03,

        AuthorizationPassword       = 0x04,
        AuthorizationComplete       = 0x05,
        AuthorizationDisconnect     = 0x06,

        ExecuteCommand              = 0x07,

        BanListRequest              = 0x08,
        BanListResponse             = 0x09,

        ChatReceivePacket           = 0x0A,
        ChatMessage                 = 0x0B,

        PlayerInfoListRequest       = 0x0C,
        PlayerInfoListResponse      = 0x0D,

        LogListRequest              = 0x0E,
        LogListResponse             = 0x0F,

        LogFileRequest              = 0x10,
        LogFileResponse             = 0x11,

        CrashLogListRequest         = 0x12,
        CrashLogListResponse        = 0x13,

        CrashLogFileRequest         = 0x14,
        CrashLogFileResponse        = 0x15,

        BanPlayer                   = 0x16,
        UnBanPlayer                 = 0x17,

        PlayerDatabaseListRequest   = 0x18,
        PlayerDatabaseListResponse  = 0x19,

        UploadLuaToServer           = 0x1A,
        ReloadNPCs                  = 0x1B,
    }
}
