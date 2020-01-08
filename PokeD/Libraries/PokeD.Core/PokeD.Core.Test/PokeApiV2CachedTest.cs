using Microsoft.VisualStudio.TestTools.UnitTesting;

using PokeD.Core.Data.PokeApi;

namespace PokeD.Core.Test
{
    [TestClass]
    public class PokeApiV2CachedTest
    {
        public PokeApiV2CachedTest()
        {
            PokeApiV2.CacheType = PokeApiV2.CacheTypeEnum.None;
        }


        [TestMethod]
        public void TestGetPokemon() { PokeApiV2Class.GetPokemon(); }

        [TestMethod]
        public void TestGetPokemonSpecies() { PokeApiV2Class.GetPokemonSpecies(); }

        [TestMethod]
        public void TestGetTypes() { PokeApiV2Class.GetTypes(); }

        [TestMethod]
        public void TestGetGender() { PokeApiV2Class.GetGender(); }

        [TestMethod]
        public void TestGetAbilities() { PokeApiV2Class.GetAbilities(); }

        [TestMethod]
        public void TestGetEggGroups() { PokeApiV2Class.GetEggGroups(); }

        [TestMethod]
        public void TestGetItems() { PokeApiV2Class.GetItems(); }

        [TestMethod]
        public void TestGetMoves() { PokeApiV2Class.GetMoves(); }

        [TestMethod]
        public void TestGetEvolutionTriggers() { PokeApiV2Class.GetEvolutionTriggers(); }
    }
}
