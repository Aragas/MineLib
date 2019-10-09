namespace MineLib.Protocol5.Protocol
{
    public enum State : byte
    {
        Handshake = 0,
        Status = 1,
        Login = 2,
        Play = 3
    }
}