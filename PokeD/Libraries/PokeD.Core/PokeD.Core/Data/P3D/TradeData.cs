using PokeD.Core.Data.PokeD;

namespace PokeD.Core.Data.P3D
{
    public class TradeData : P3DData
    {
        public static implicit operator TradeData(string tradeData) => new TradeData(tradeData);

        public Monster Monster => new Monster(Data);

        public TradeData(string tradeData) : base(tradeData) { }
    }
}