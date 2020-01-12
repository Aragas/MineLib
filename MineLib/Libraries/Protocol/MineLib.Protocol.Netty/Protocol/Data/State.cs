namespace MineLib.Protocol.Netty
{
    public enum State : byte
    {
        Handshake = 0,
        Status = 1,
        Login = 2,
        Play = 3
    }
}