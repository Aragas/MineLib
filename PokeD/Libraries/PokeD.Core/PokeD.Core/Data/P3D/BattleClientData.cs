namespace PokeD.Core.Data.P3D
{
    public class BattleClientData : P3DData
    {
        public static implicit operator BattleClientData(string battleData) => new BattleClientData(battleData);

        public string Action => Data.Split('|')[0] ?? string.Empty;
        public string ActionValue => Data.Split('|')[1] ?? string.Empty;

        public BattleClientData(string battleData) : base(battleData) { }
    }
}