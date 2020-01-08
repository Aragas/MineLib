using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PokeD.Core.Test
{
    [TestClass]
    public class MonsterTest
    {
        [TestMethod]
        public void TestMonsterDataItems() { MonsterClass.MonsterDataItems(); }

        [TestMethod]
        public void TestLoadStaticData() { MonsterClass.LoadStaticData(); }
    }
}
