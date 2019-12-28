namespace MineLib.Core.Anvil
{
    public interface IBlock
    {
        uint ID { get; }
        byte Metadata { get; }
        byte Light { get; }
        byte SkyLight { get; }
    }
}