﻿namespace PokeD.Server.Proxy.Protocol.Data
{
    internal enum State : byte
    {
        Handshake = 0,
        Status = 1,
        Login = 2,
        Play = 3
    }
}