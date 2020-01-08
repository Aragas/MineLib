using PokeD.Core.Data.P3D;

namespace PokeD.Core.Test
{
    public static class MonsterClass
    {
        private const int MaxPokemon = 721;

        private static readonly DataItems TestDataItems = new DataItems(@"{""Pokemon""[152]}{""Experience""[607]}{""Gender""[1]}{""EggSteps""[0]}{""Item""[0]}{""ItemData""[]}{""NickName""[Elia]}{""Level""[10]}{""OT""[29388]}{""Ability""[65]}{""Status""[]}{""Nature""[24]}{""CatchLocation""[at New Bark Town]}{""CatchTrainer""[Aragas]}{""CatchBall""[5]}{""CatchMethod""[As a gift from Prof. Elm]}{""Friendship""[112]}{""isShiny""[0]}{""Attack1""[33,35,35]}{""Attack2""[45,40,40]}{""Attack3""[75,25,25]}{""Attack4""[77,35,35]}{""HP""[31]}{""EVs""[6,4,0,0,2,12]}{""IVs""[26,22,13,17,9,10]}{""AdditionalData""[]}{""IDValue""[PokeD01Conv]}");
        /*
        private static readonly Monster TestMonster = new Monster(new MonsterInstanceData(152, MonsterGender.Male, false, 65, 24)
        {
            Experience = 607,
            EggSteps = 0,
            HeldItem = 0,
            CatchInfo = new MonsterCatchInfo { Nickname = "Elia", TrainerID = 29388, Location  = "at New Bark Town", TrainerName = "Aragas", PokeballID = 5, Method = "As a gift from Prof. Elm" },
            Friendship = 112,
            Moves = new MonsterMoves(new MonsterMove(33, 0), new MonsterMove(45, 0), new MonsterMove(75, 0), new MonsterMove(77, 0)),
            CurrentHP = 31,
            EV = new MonsterStats(6, 4, 0, 0, 2, 12),
            IV = new MonsterStats(26, 22, 13, 17, 9, 10)
        });
        */


        public static void MonsterDataItems()
        {
            /*
            var monster = TestDataItems.ToMonster();
            var monsterDataItems = monster.ToDataItems();
            var testMonsterDataItems = TestMonster.ToDataItems();

            var testDataItemsString = TestDataItems.ToString();
            var monsterString = monsterDataItems.ToString();
            var testMonsterDataItemsString = testMonsterDataItems.ToString();

            Assert.IsTrue(testDataItemsString == monsterString);
            Assert.IsTrue(testMonsterDataItemsString == testDataItemsString);
            */
        }

        public static void LoadStaticData() { /* MonsterStaticData.LoadStaticDataPokeApiV2((short) new Random().Next(1, MaxPokemon)).Wait(); */ }
    }
}
