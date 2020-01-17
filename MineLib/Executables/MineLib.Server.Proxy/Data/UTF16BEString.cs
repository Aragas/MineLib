namespace MineLib.Server.Proxy.Data
{
    public class UTF16BEString
    {
        public static implicit operator string(UTF16BEString str) => str.Value;
        public static implicit operator UTF16BEString(string str) => new UTF16BEString(str);

        private string Value { get; }

        public UTF16BEString(string value) => Value = value;

        public override string ToString() => Value;
    }
}