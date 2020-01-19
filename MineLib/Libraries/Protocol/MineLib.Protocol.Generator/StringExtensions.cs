namespace MineLib.Protocol.Generator
{
    internal static class StringExtensions
    {
        public static string Sanitize(this string str) => str
            .Trim()
            .Replace("\n", "");
    }
}
