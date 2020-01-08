using Microsoft.VisualStudio.TestTools.UnitTesting;

using PokeD.Core.Data.PokeApi;

namespace PokeD.Core.Test
{
    [TestClass]
    public class MonsterCachedTest
    {
        public MonsterCachedTest()
        {
            PokeApiV2.CacheType = PokeApiV2.CacheTypeEnum.None;
        }


        [TestMethod]
        public void TestMonsterDataItems() { MonsterClass.MonsterDataItems(); }

        [TestMethod]
        public void TestLoadStaticData() { MonsterClass.LoadStaticData(); }
    }
}
