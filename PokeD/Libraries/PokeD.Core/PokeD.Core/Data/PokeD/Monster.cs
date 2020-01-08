using System;
using System.Collections.Generic;
using System.Linq;

using MersenneTwister;

using PokeD.BattleEngine.Ability;
using PokeD.BattleEngine.Attack;
using PokeD.BattleEngine.Attack.Data;
using PokeD.BattleEngine.Item.Data;
using PokeD.BattleEngine.Monster;
using PokeD.BattleEngine.Monster.Data;
using PokeD.BattleEngine.Monster.Enums;
using PokeD.Core.Data.P3D;
using PokeD.Core.Extensions;

namespace PokeD.Core.Data.PokeD
{
    public sealed class Monster : BaseMonsterInstance
    {
        public ushort SecretID { get; }
        public uint PersonalityValue { get; }


        public string DisplayName => !string.IsNullOrWhiteSpace(CatchInfo.Nickname) ? CatchInfo.Nickname : StaticData.Name;


        public override Gender Gender => StaticData.MaleRatio < 0.0f ? Gender.Genderless : (PersonalityValue % 256 < (byte) (StaticData.MaleRatio * byte.MaxValue) ? Gender.Female : Gender.Male);

        public bool FirstAbility => (PersonalityValue / 65536) % 2 == 0;
        public override bool IsShiny => (PersonalityValue % 65536) < 16;
        public byte Characteristic => (byte)(PersonalityValue % 6);
        public override BaseAbility Ability => FirstAbility ? StaticData.Abilities.Ability_0 : StaticData.Abilities.Ability_1;

        public (int X, int Y)[] SpindaSpots => BitConverter.GetBytes(PersonalityValue).Select(b => (b & 0x0F, b >> 4)).ToArray();
        public byte WurmplesEvolution => (byte) (PersonalityValue / 65536);


        public Monster(short species) : base(Cached<MonsterStaticData>.Get(species))
        {
            var random = new MersenneTwisterRandom();
            var thirtyBits = (uint) random.Next(1 << 30);
            var twoBits = (uint) random.Next(1 << 2);
            PersonalityValue = (thirtyBits << 2) | twoBits;

            Nature = GenerateNature();

            IV = new Stats(
                (short) random.Next(0, 31),
                (short) random.Next(0, 31),
                (short) random.Next(0, 31),
                (short) random.Next(0, 31),
                (short) random.Next(0, 31),
                (short) random.Next(0, 31));
            
            /*
            First Random Number:    x|xxxxx|xxxxx|xxxxx
                                    -|DefIV|AtkIV|HP IV

            Second Random Number:   x|xxxxx|xxxxx|xxxxx
                                    -|SpDIV|SpAIV|SpeIV
             */
        }
        public Monster(short species, ushort secretID, uint personalityValue, byte nature) : base(Cached<MonsterStaticData>.Get(species))
        {
            SecretID = secretID;
            PersonalityValue = personalityValue;

            Nature = nature;
        }
        public Monster(short species, Gender gender, bool isShiny, short ability, byte nature) : base(Cached<MonsterStaticData>.Get(species))
        {
            PersonalityValue = StaticData.Abilities.Contains(ability) ? GenerateRandom(gender, isShiny, ability) : 0;
            Nature = nature;
        }
        private uint GenerateRandom(Gender gender, bool isShiny, short ability)
        {
            int cycles = 0;

            Generate:
            if (cycles > 100)
                return 0;
            cycles++;

            var random = new MersenneTwisterRandom();
            var thirtyBits = (uint) random.Next(1 << 30);
            var twoBits = (uint) random.Next(1 << 2);
            var result = (thirtyBits << 2) | twoBits;

            Check:
            var cGender = StaticData.MaleRatio < 0.0f ? Gender.Genderless : (result % 256 < (byte) (StaticData.MaleRatio * byte.MaxValue) ? Gender.Male : Gender.Female);
            var cFirstAbility = (result / 65536 % 2) == 0;
            var cIsShiny = (result % 65536) < 16;

            if (gender != cGender || ((ability == StaticData.Abilities.Ability_0.StaticData.ID) != cFirstAbility) || isShiny != cIsShiny)
                goto Generate;

            return result;
        }
        private static byte GenerateNature() => (byte) new MersenneTwisterRandom().Next(25);



        public Monster(Monster father, Monster mother) : base(GetPreEvolution(mother.StaticData, father.StaticData))
        {
            
            InheritAttacks(father, mother, this);


            // -- Characteristic
            byte characteristic = InheritCharacteristic(father, mother, this);

            // -- Characteristic


        }
        private static IMonsterStaticData GetPreEvolution(IMonsterStaticData father, IMonsterStaticData mother)
        {
            // Baby/Ditto check.

            return mother;
        }
        private static bool CanPassDownNature(Monster monster) => monster.HeldItem.StaticData.Attributes.Any(att => att is ItemAttributePassDownNature);
        private static bool IsDitto(IMonsterStaticData monster) => false;
        private static void InheritAttacks(Monster father, Monster mother, Monster child)
        {
            var attacks = child.StaticData.LearnableAttacks.Where(attack => (attack.LearnCondition as AttackLearn.ByLevel)?.Level == 1).Select(la => la.Attack).ToList();
            var sharedAttacks = father.Moves.Intersect(mother.Moves).Select(attack => attack.StaticData);
            var learnableAttacks = child.StaticData.LearnableAttacks.Select(la => la.Attack).Select(la => sharedAttacks.Contains(la)).ToList();
        }
        private static void InheritAbility(Monster father, Monster mother, Monster child)
        {
            

        }
        private static byte InheritCharacteristic(Monster father, Monster mother, Monster child)
        {
            if (CanPassDownNature(father) && CanPassDownNature(mother))
            {
                switch (new Random().Next(0, 1))
                {
                    case 0:
                        return father.Characteristic;
                    case 1:
                        return mother.Characteristic;
                }
            }

            if (CanPassDownNature(father))
                return father.Characteristic;
            if (CanPassDownNature(mother))
                return mother.Characteristic;

            throw new Exception();
        }



        public Monster(DataItems dataItems) : this(GetID(dataItems), GetGender(dataItems), GetIsShiny(dataItems), GetAbility(dataItems), GetNature(dataItems))
        {
            var dict = dataItems.ToDictionary();


            Experience = int.Parse(dict["Experience"]);
            Friendship = byte.Parse(dict["Friendship"]);
            EggSteps = int.Parse(dict["EggSteps"]);
            CatchInfo = new CatchInfo()
            {
                Nickname = string.IsNullOrEmpty(dict["NickName"]) ? string.Empty : dict["NickName"],
                PokeballID = byte.Parse(dict["CatchBall"]),
                Method = dict["CatchMethod"],
                Location = dict["CatchLocation"],
                TrainerName = dict["CatchTrainer"],
                TrainerID = (ushort) int.Parse(dict["OT"]).BitsGet(0, 16) == ushort.MaxValue ? (ushort) int.Parse(dict["OT"]).BitsGet(16, 32) : (ushort) int.Parse(dict["OT"]).BitsGet(0, 16)
            };

            if(short.TryParse(dict["Item"], out var item) && item != 0)
                HeldItem = new Item(item);

            var move0 = dict["Attack1"].Split(',');
            var move1 = dict["Attack2"].Split(',');
            var move2 = dict["Attack3"].Split(',');
            var move3 = dict["Attack4"].Split(',');
            Moves = new List<BaseAttackInstance>();
            if (move0.Length != 1)
            {
                var dat = Cached<AttackStaticData>.Get(short.Parse(move0[0]));
                var ppUps = (byte) Math.Round((double) (byte.Parse(move0[1]) - dat.PP) / dat.PP / 0.2D);
                Moves.Add(new Attack(dat, byte.Parse(move0[2]), ppUps));
            }
            if (move1.Length != 1)
            {
                var dat = Cached<AttackStaticData>.Get(short.Parse(move1[0]));
                var ppUps = (byte) Math.Round((double) (byte.Parse(move1[1]) - dat.PP) / dat.PP / 0.2D);
                Moves.Add(new Attack(dat, byte.Parse(move1[2]), ppUps));
            }
            if (move2.Length != 1)
            {
                var dat = Cached<AttackStaticData>.Get(short.Parse(move2[0]));
                var ppUps = (byte) Math.Round((double) (byte.Parse(move2[1]) - dat.PP) / dat.PP / 0.2D);
                Moves.Add(new Attack(dat, byte.Parse(move2[2]), ppUps));
            }
            if (move3.Length != 1)
            {
                var dat = Cached<AttackStaticData>.Get(short.Parse(move3[0]));
                var ppUps = (byte) Math.Round((double) (byte.Parse(move3[1]) - dat.PP) / dat.PP / 0.2D);
                Moves.Add(new Attack(dat, byte.Parse(move3[2]), ppUps));
            }

            CurrentHP = short.Parse(dict["HP"]);

            var ev = dict["EVs"].Split(',');
            var ev0 = short.Parse(ev[0]);
            var ev1 = short.Parse(ev[1]);
            var ev2 = short.Parse(ev[2]);
            var ev3 = short.Parse(ev[3]);
            var ev4 = short.Parse(ev[4]);
            var ev5 = short.Parse(ev[5]);
            EV = new Stats(ev0, ev1, ev2, ev3, ev4, ev5);

            var iv = dict["IVs"].Split(',');
            var iv0 = short.Parse(iv[0]);
            var iv1 = short.Parse(iv[1]);
            var iv2 = short.Parse(iv[2]);
            var iv3 = short.Parse(iv[3]);
            var iv4 = short.Parse(iv[4]);
            var iv5 = short.Parse(iv[5]);
            IV = new Stats(iv0, iv1, iv2, iv3, iv4, iv5);
        }
        private static short GetID(DataItems dataItems)
        {
            var dict = dataItems.ToDictionary();
            return short.Parse(dict["Pokemon"]);
        }
        private static Gender GetGender(DataItems dataItems)
        {
            var dict = dataItems.ToDictionary();
            return (int.Parse(dict["Gender"])) switch
            {
                0 => Gender.Male,
                1 => Gender.Female,
                2 => Gender.Genderless,
                _ => Gender.Genderless,
            };
        }
        private static bool GetIsShiny(DataItems dataItems)
        {
            var dict = dataItems.ToDictionary();
            return int.Parse(dict["isShiny"]) != 0;
        }
        private static short GetAbility(DataItems dataItems)
        {
            var dict = dataItems.ToDictionary();
            return short.Parse(dict["Ability"]);

        }
        private static byte GetNature(DataItems dataItems)
        {
            var dict = dataItems.ToDictionary();
            return byte.Parse(dict["Nature"]);
        }


        public bool IsValid { get; private set; }
    }
}
