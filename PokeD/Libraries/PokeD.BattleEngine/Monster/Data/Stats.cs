using System;

using PokeD.BattleEngine.Monster.Enums;

namespace PokeD.BattleEngine.Monster.Data
{
    public class Stats
    {
        public static Stats None => new Stats(0, 0, 0, 0, 0, 0);

        public short HP { get; }
        public short Attack { get; }
        public short Defense { get; }
        public short SpecialAttack { get; }
        public short SpecialDefense { get; }
        public short Speed { get; }

        public short this[StatType statType]
        {
            get
            {
                switch (statType)
                {
                    case StatType.HP:
                        return HP;
                    case StatType.Attack:
                        return Attack;
                    case StatType.Defense:
                        return Defense;
                    case StatType.SpecialAttack:
                        return SpecialAttack;
                    case StatType.SpecialDefense:
                        return SpecialDefense;
                    case StatType.Speed:
                        return Speed;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            /*
            set
            {
                switch (statType)
                {
                    case MonsterStatType.HP:
                        HP = value;
                        break;
                    case MonsterStatType.Attack:
                        Attack = value;
                        break;
                    case MonsterStatType.Defense:
                        Defense = value;
                        break;
                    case MonsterStatType.SpecialAttack:
                        SpecialAttack = value;
                        break;
                    case MonsterStatType.SpecialDefense:
                        SpecialDefense = value;
                        break;
                    case MonsterStatType.Speed:
                        Speed = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            */
        }
        
        public Stats(short hp, short attack, short defense, short specialAttack, short specialDefense, short speed)
        {
            HP = hp;
            Attack = attack;
            Defense = defense;
            SpecialAttack = specialAttack;
            SpecialDefense = specialDefense;
            Speed = speed;
        }
        
        public override string ToString() => $"HP: {HP}, Atk: {Attack}, Def: {Defense}, SpAtt: {SpecialAttack}, SpDef: {SpecialDefense}, Spe: {Speed}";

        public short GetStat(StatType statType)
        {
            switch (statType)
            {
                case StatType.HP:
                    return HP;
                case StatType.Attack:
                    return Attack;
                case StatType.Defense:
                    return Defense;
                case StatType.SpecialAttack:
                    return SpecialAttack;
                case StatType.SpecialDefense:
                    return SpecialDefense;
                case StatType.Speed:
                    return Speed;
            }

            return 0;
        }

        public bool IsValidIV()
        {
            return (HP >= 0 && HP <= 31) &&
                   (Attack >= 0 && Attack <= 31) &&
                   (Defense >= 0 && Defense <= 31) &&
                   (SpecialAttack >= 0 && SpecialAttack <= 31) &&
                   (SpecialDefense >= 0 && SpecialDefense <= 31) &&
                   (Speed >= 0 && Speed <= 31);
        }

        public bool IsValidEV()
        {
            // TODO: 255 or 252?
            return (HP >= 0 && HP <= 255) &&
                   (Attack >= 0 && Attack <= 255) &&
                   (Defense >= 0 && Defense <= 255) &&
                   (SpecialAttack >= 0 && SpecialAttack <= 255) &&
                   (SpecialDefense >= 0 && SpecialDefense <= 255) &&
                   (Speed >= 0 && Speed <= 255) &&
                   (HP + Attack + Defense + SpecialAttack + SpecialDefense + Speed <= 510);
        }
    }
}