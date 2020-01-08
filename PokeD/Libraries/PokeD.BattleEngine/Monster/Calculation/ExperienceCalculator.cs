using System;
using System.Linq;

using PokeD.BattleEngine.Item.Data;
using PokeD.BattleEngine.Trainer;
using PokeD.BattleEngine.Monster.Data;
using PokeD.BattleEngine.Monster.Enums;

namespace PokeD.BattleEngine.Monster.Calculation
{
    public static class ExperienceCalculator
    {
        public static int GainExperience(BaseTrainerInstance trainer, BaseMonsterInstance victorious, BaseMonsterInstance fainted, bool hasParticipatedAndExpShare = false, bool useScaled = false)
        {
            double a = fainted.CatchInfo.TrainerID == 0 ? 1D : 1.5D; // is wild
            double b = fainted.StaticData.RewardExperience;
            double e = victorious.HeldItem.StaticData.Attributes.Any(att => att is ItemAttributeBonusExp) ? 1.5D : 0D;
            double f = victorious.Affection >= 2 ? 1.2D : 1D;
            double l = fainted.Level;
            double lp = victorious.Level;
            double p = 1D;
            double s = hasParticipatedAndExpShare ? 2D : 1D;
            double t = trainer.ID == victorious.CatchInfo.TrainerID ? 1.5D : 1D;
            var minLevelEvolveRequirement = victorious.StaticData.EvolvesTo.Min(et => et.EvolutionConditions.Min(ec => ec.SubConditions.Where(sc => sc is EvolvesTo.ByLevel).Cast<EvolvesTo.ByLevel>().Min(sc => sc.Level)));
            double v = victorious.Level > minLevelEvolveRequirement ? 1.2 : 1;
            //double v = 1;

            if (!useScaled)
                return (int) ((a * t * b * e * l * p * f * v) / 7D * s);
            else
                return (int) ((((a * b * l) / 5D * s) * (Math.Pow(2D * l + 10D, 2.5D) / Math.Pow(l + lp + 10D, 2.5D)) + 1D) * t * e * p);
        }

        public static byte LevelForExperienceValue(ExperienceType experienceType, long experience)
        {
            // returns level 1 if no experience (or negative value):
            if (experience <= 0)
                return 1;

            byte level = 1;
            while (ExperienceNeededForLevel(experienceType, level) < experience)
                level++;
            level--;

            return level;
        }
        public static int ExperienceNeededForLevel(ExperienceType experienceType, int level)
        {
            switch (experienceType)
            {
                case ExperienceType.Erratic:
                    return (int) Math.Round(ExperienceNeededForLevelErratic(level));
                case ExperienceType.Fast:
                    return (int) Math.Round(ExperienceNeededForLevelFast(level));
                case ExperienceType.MediumFast:
                    return (int) Math.Round(ExperienceNeededForLevelMediumFast(level));
                case ExperienceType.MediumSlow:
                    return (int) Math.Round(ExperienceNeededForLevelMediumSlow(level));
                case ExperienceType.Slow:
                    return (int) Math.Round(ExperienceNeededForLevelSlow(level));
                case ExperienceType.Fluctuating:
                    return (int) Math.Round(ExperienceNeededForLevelFluctuating(level));
                default:
                    return (int) Math.Round(ExperienceNeededForLevelMediumFast(level));
            }
        }
        
        private static double ExperienceNeededForLevelErratic(int level)
        {
            // EXP = 
            // level <= 50:         ((pow(level, 3) * (100 - level)) / 50)
            // 50 <= level <= 68:   ((pow(level, 3) * (150 - level)) / 100)
            // 68 <= level <= 98:   ((pow(level, 3) * floor((1911 - (10 * level)) / 3)) / 500)
            // 98 <= level <= 100:  ((pow(level, 3) * (160 - level)) / 100)

            if (level <= 50)
                return (Math.Pow(level, 3D) * (100D - level)) / 50D;
            else if (50 <= level && level <= 68)
                return (Math.Pow(level, 3D) * (150D - level)) / 100D;
            else if (68 <= level && level <= 98)
                return (Math.Pow(level, 3D) * Math.Floor((1911D - (10D * level)) / 3D)) / 500D;
            else if (98 <= level && level <= 100)
                return (Math.Pow(level, 3D) * (160D - level)) / 100D;
            else
                throw new Exception();
        }
        private static double ExperienceNeededForLevelFast(int level)
        {
            // EXP = 
            // ((4 * pow(level, 3)) / 5)

            return (4D * Math.Pow(level, 3D)) / 5D;
        }
        private static double ExperienceNeededForLevelMediumFast(int level)
        {
            // EXP =
            // (pow(level, 3))
            var t = Math.Pow(level, 3D);

            return Math.Pow(level, 3D);
        }
        private static double ExperienceNeededForLevelMediumSlow(int level)
        {
            // EXP = 
            // (((6 / 5) * pow(level, 3)) - (15 * pow(level, 2)) + (100 * level) - 140)

            return ((6.0D / 5.0D) * Math.Pow(level, 3D)) - (15D * Math.Pow(level, 2D)) + (100D * level) - 140D;
        }
        private static double ExperienceNeededForLevelSlow(int level)
        {
            // EXP = 
            // ((5 * pow(level, 3)) / 4)

            return (5D * Math.Pow(level, 3D)) / 4D;
        }
        private static double ExperienceNeededForLevelFluctuating(int level)
        {
            // EXP = 
            // level <= 15: (pow(level, 3) * ((floor((level + 1) / 3) + 24) / 50))
            // 15 <= level <= 36: (pow(level, 3) * ((level + 14) / 50))
            // 36 <= level <= 100: (pow(level, 3) * ((floor(level / 2) + 32) / 50))

            if (level <= 15)
                return Math.Pow(level, 3D) * ((Math.Floor((level + 1D) / 3D) + 24D) / 50D);
            else if (15 <= level && level <= 36)
                return Math.Pow(level, 3D) * ((level + 14D) / 50D);
            else if (36 <= level && level <= 100)
                return Math.Pow(level, 3D) * ((Math.Floor(level / 2D) + 32D) / 50D);
            else
                throw new Exception();
        }
    }
}