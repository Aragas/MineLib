namespace PokeD.Core.Packets.PokeD
{
    public enum PokeDPacketTypes
    {
        AuthorizationRequest    = 0xA1,
        AuthorizationResponse   = 0xA2,
        AuthorizationDisconnect = 0xA3,
        AuthorizationComplete   = 0xA4,
        EncryptionRequest       = 0xA5,
        EncryptionResponse      = 0xA6,


        BattleRequest           = 0xB0,
        BattleCancelled         = 0xB1,
        BattleOffer             = 0xB2,
        BattleAccept            = 0xB3,
        
        BattleAttack            = 0xB4,
        BattleItem              = 0xB5,
        BattleSwitch            = 0xB6,
        BattleFlee              = 0xB7,
        
        BattleState             = 0xB8,


        ChatGlobalMessage       = 0xC0,
        ChatPrivateMessage      = 0xC1,
        ChatServerMessage       = 0xC2,


        Ping                    = 0xD0,
        Position                = 0xD1,
        TrainerInfo             = 0xD2,
        Map                     = 0xD3,
        TileSetRequest          = 0xD4,
        TileSetResponse         = 0xD5,
        WorldInfo               = 0xD6,


        TradeOffer              = 0xE0,
        TradeAccept             = 0xE1,
        TradeRefuse             = 0xE2,

        Disconnect              = 0xFF,
    }
}
