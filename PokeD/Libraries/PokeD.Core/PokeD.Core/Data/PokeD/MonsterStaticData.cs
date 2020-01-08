using System;
using System.Collections.Generic;
using System.Linq;

using PokeAPI;

using PokeD.BattleEngine.Ability;
using PokeD.BattleEngine.Attack.Data;
using PokeD.BattleEngine.Battle.Data;
using PokeD.BattleEngine.EggGroup;
using PokeD.BattleEngine.Item;
using PokeD.BattleEngine.Monster;
using PokeD.BattleEngine.Monster.Data;
using PokeD.BattleEngine.Monster.Enums;
using PokeD.BattleEngine.Type;
using PokeD.Core.Data.PokeApi;

namespace PokeD.Core.Data.PokeD
{
    public class MonsterStaticData : IMonsterStaticData
    {
        public static Languages Language { get; set; } = Languages.English;
        internal static bool GetLocalizedName(ResourceName resourceName) => resourceName.Language.ID == (int) Language;

        public short ID { get; }
        public string Name { get; }

        public Types Types { get; }
        public Abilities Abilities { get; }
        public EggGroups EggGroups { get; }
        public IReadOnlyList<IItemStaticData> HeldItems { get; }

        public int Height { get; }
        public int Weight { get; }
        
        public Stats BaseStats { get; }
        public byte BaseHappiness { get; }
        
        public byte CatchRate { get; }

        public float MaleRatio { get; }

        public int HatchCycles { get; }

        public bool IsBaby { get; }

        public bool HasGenderDifferences { get; }

        private Habitat _habitat;
        public Habitat Habitat
        {
            get
            {
                if (_habitat == null)
                {
                    var habitat = DataFetcher.GetApiObject<PokemonSpecies>(ID).Result.Habitat?.GetObject().Result;
                    _habitat = habitat != null ? new Habitat(habitat.ID, habitat.Names.Single(GetLocalizedName).Name) : Habitat.None;
                }
                return _habitat;
            }
        }

        private Color _color;
        public Color Color
        {
            get
            {
                if (_color == null)
                {
                    var color = DataFetcher.GetApiObject<PokemonSpecies>(ID).Result.Colours.GetObject().Result;
                    _color = new Color(color.ID, color.Names.Single(GetLocalizedName).Name);
                }
                return _color;
            }
        }

        private Shape _shape;
        public Shape Shape
        {
            get
            {
                if (_shape == null)
                {
                    var shape = DataFetcher.GetApiObject<PokemonSpecies>(ID).Result.Shape.GetObject().Result;
                    _shape = new Shape(shape.ID, shape.Names.Single(GetLocalizedName).Name);
                }
                return _shape;
            }
        }

        public ExperienceType ExperienceType { get; }

        public short RewardExperience { get; }
        public Stats RewardStats { get; }

        private IReadOnlyList<EvolvesTo> _evolvesTo;
        public IReadOnlyList<EvolvesTo> EvolvesTo => _evolvesTo ?? (_evolvesTo = GetEvolutions());

        private IReadOnlyList<AttackLearn> _learnableAttacks;
        public IReadOnlyList<AttackLearn> LearnableAttacks => _learnableAttacks ?? (_learnableAttacks = GetLearnableAttacks());
        
        public MonsterStaticData(short id)
        {
            var pokemon             = DataFetcher.GetApiObject<Pokemon>(id).Result;
            var pokemonSpecies      = DataFetcher.GetApiObject<PokemonSpecies>(id).Result;


            ID = id;
            Name = pokemonSpecies.Names.Single(GetLocalizedName).Name;

            Types = new Types(pokemon.Types.Select(type => Cached<MonsterTypeStaticData>.Get((byte) type.Type.ID)).ToArray<ITypeStaticData>());
            Abilities = new Abilities(pokemon.Abilities.Select(ability => new Ability((short) ability.Ability.ID, ability.IsHidden)).ToArray<BaseAbility>());
            EggGroups = new EggGroups(pokemonSpecies.EggGroups.Select(eggGroup => Cached<EggGroupStaticData>.Get((byte) eggGroup.ID)).ToArray<IEggGroupStaticData>());
            HeldItems = pokemon.HeldItems.Select(heldItem => Cached<ItemStaticData>.Get(heldItem.Item.ID)).ToList<IItemStaticData>();

            Height = pokemon.Height;
            Weight = pokemon.Mass;

            BaseStats = new Stats(
                (short) pokemon.Stats.Single(stat => stat.Stat.ID == (int) StatType.HP).BaseValue,
                (short) pokemon.Stats.Single(stat => stat.Stat.ID == (int) StatType.Attack).BaseValue,
                (short) pokemon.Stats.Single(stat => stat.Stat.ID == (int) StatType.Defense).BaseValue,
                (short) pokemon.Stats.Single(stat => stat.Stat.ID == (int) StatType.SpecialAttack).BaseValue,
                (short) pokemon.Stats.Single(stat => stat.Stat.ID == (int) StatType.SpecialDefense).BaseValue,
                (short) pokemon.Stats.Single(stat => stat.Stat.ID == (int) StatType.Speed).BaseValue);
            BaseHappiness = (byte) pokemonSpecies.BaseHappiness;

            CatchRate = (byte) pokemonSpecies.CaptureRate;

            MaleRatio = 1.0f - pokemonSpecies.FemaleToMaleRate ?? -1.0f;

            HatchCycles = (byte) pokemonSpecies.HatchCounter;

            IsBaby = pokemonSpecies.IsBaby;

            HasGenderDifferences = pokemonSpecies.HasGenderDifferences;

            ExperienceType = (ExperienceType) pokemonSpecies.GrowthRate.ID;

            RewardExperience = (short) pokemon.BaseExperience;
            RewardStats = new Stats(
                (short) pokemon.Stats.Single(stat => stat.Stat.ID == (int) StatType.HP).Effort,
                (short) pokemon.Stats.Single(stat => stat.Stat.ID == (int) StatType.Attack).Effort,
                (short) pokemon.Stats.Single(stat => stat.Stat.ID == (int) StatType.Defense).Effort,
                (short) pokemon.Stats.Single(stat => stat.Stat.ID == (int) StatType.SpecialAttack).Effort,
                (short) pokemon.Stats.Single(stat => stat.Stat.ID == (int) StatType.SpecialDefense).Effort,
                (short) pokemon.Stats.Single(stat => stat.Stat.ID == (int) StatType.Speed).Effort);
        }

        private IReadOnlyList<EvolvesTo> GetEvolutions()
        {
            var evolvesToList = new List<EvolvesTo>();
            var evolutionChain = DataFetcher.GetApiObject<PokemonSpecies>(ID).Result.EvolutionChain.GetObject().Result;
            if (evolutionChain.Chain.EvolvesTo.Any())
            {
                var chains = GetChains(evolutionChain.Chain);
                var chain = chains.FirstOrDefault(c => ID == c.Species.ID);
                foreach (var evolvesTo in chain.EvolvesTo)
                {
                    var list = new List<EvolvesTo.IEvolutionCondition>();
                    foreach (var evolutionDetail in evolvesTo.Details)
                    {
                        var evolutionConditions = new List<EvolvesTo.ISubEvolutionCondition>();

                        if (evolutionDetail.MinLevel != null)
                            evolutionConditions.Add(new EvolvesTo.ByLevel((byte) evolutionDetail.MinLevel));

                        if (evolutionDetail.MinHappiness != null)
                            evolutionConditions.Add(new EvolvesTo.ByHappiness((byte) evolutionDetail.MinHappiness.Value));

                        if (evolutionDetail.MinAffection != null)
                            evolutionConditions.Add(new EvolvesTo.ByAffection((byte) evolutionDetail.MinAffection.Value));

                        if (evolutionDetail.MinBeauty != null)
                            evolutionConditions.Add(new EvolvesTo.ByBeauty((byte) evolutionDetail.MinBeauty));

                        if (evolutionDetail.KnownMove != null)
                            evolutionConditions.Add(new EvolvesTo.ByAttack(Cached<AttackStaticData>.Get((short) evolutionDetail.KnownMove.ID)));

                        if (evolutionDetail.KnownMoveType != null)
                            evolutionConditions.Add(new EvolvesTo.ByKnownAttackType(Cached<MonsterTypeStaticData>.Get((byte) evolutionDetail.KnownMoveType.ID)));

                        if (evolutionDetail.Item != null)
                            evolutionConditions.Add(new EvolvesTo.ByItem(Cached<ItemStaticData>.Get(evolutionDetail.Item.ID)));

                        if (evolutionDetail.HeldItem != null)
                            evolutionConditions.Add(new EvolvesTo.ByHeldItem(Cached<ItemStaticData>.Get(evolutionDetail.HeldItem.ID)));

                        if (evolutionDetail.Gender != null)
                            evolutionConditions.Add(new EvolvesTo.ByGender((BattleEngine.Monster.Enums.Gender) evolutionDetail.Gender));

                        if (evolutionDetail.Location != null)
                            evolutionConditions.Add(new EvolvesTo.ByArea(null));

                        if (evolutionDetail.NeedsOverworldRain)
                            evolutionConditions.Add(new EvolvesTo.ByWeather(global::PokeD.BattleEngine.Battle.Data.Weather.Rain));

                        if (evolutionDetail.PartySpecies != null)
                            evolutionConditions.Add(new EvolvesTo.ByMonsterInTeam((short) evolutionDetail.PartySpecies.ID));

                        if (evolutionDetail.PartyType != null)
                            evolutionConditions.Add(new EvolvesTo.ByMonsterTypeInTeam(Cached<MonsterTypeStaticData>.Get((byte) evolutionDetail.PartySpecies.ID)));

                        if (evolutionDetail.TradeSpecies != null)
                            evolutionConditions.Add(new EvolvesTo.ByTradeMonster((short) evolutionDetail.TradeSpecies.ID));

                        if (evolutionDetail.TurnUpsideDown)
                            evolutionConditions.Add(new ByTurnUpsideDown());

                        if (!string.IsNullOrEmpty(evolutionDetail.TimeOfDay))
                        {
                            if (Enum.TryParse(evolutionDetail.TimeOfDay, true, out EvolvesTo.ByTimeOfDay.TimeOfDay @enum))
                                evolutionConditions.Add(new EvolvesTo.ByTimeOfDay(@enum));
                            else
                                throw new Exception();
                        }

                        switch (evolutionDetail.Trigger.ID)
                        {
                            case 1:
                                list.Add(new EvolvesTo.ByLevelUp(evolutionConditions));
                                break;

                            case 2:
                                list.Add(new EvolvesTo.ByTrade(evolutionConditions));
                                break;

                            case 3:
                                list.Add(new EvolvesTo.ByItemUse(evolutionConditions));
                                break;

                            case 4:
                                list.Add(new ByShed());
                                break;

                            default:
                                break;
                        }
                    }

                    evolvesToList.Add(new EvolvesTo(Cached<MonsterStaticData>.Get((short) evolvesTo.Species.ID), list));
                }
            }
            return evolvesToList;
        }
        private static IReadOnlyList<ChainLink> GetChains(ChainLink chain)
        {
            var list = new List<ChainLink> { chain };
            foreach (var evolves_to in chain.EvolvesTo)
                list.AddRange(GetChains(evolves_to));
            return list;
        }

        private IReadOnlyList<AttackLearn> GetLearnableAttacks()
        {
            var learnableAttacksList = new List<AttackLearn>();
            foreach (var move in DataFetcher.GetApiObject<Pokemon>(ID).Result.Moves)
            {
                if (move.VersionGroupDetails.Any())
                {
                    var vg = move.VersionGroupDetails.First();
                    switch (vg.LearnMethod.ID)
                    {
                        case 1:
                            learnableAttacksList.Add(new AttackLearn(Cached<AttackStaticData>.Get((short) move.Move.ID), new AttackLearn.ByLevel((byte) vg.LearnedAt)));
                            break;

                        case 2:
                            learnableAttacksList.Add(new AttackLearn(Cached<AttackStaticData>.Get((short) move.Move.ID), new AttackLearn.ByBreeding()));
                            break;

                        case 3:
                            learnableAttacksList.Add(new AttackLearn(Cached<AttackStaticData>.Get((short) move.Move.ID), new AttackLearn.ByTutor()));
                            break;

                        case 4:
                            learnableAttacksList.Add(new AttackLearn(Cached<AttackStaticData>.Get((short) move.Move.ID), new AttackLearn.ByMachine()));
                            break;
                        // Else is shit
                    }
                }
            }
            return learnableAttacksList;
        }

        public override string ToString() => $"{Name }"; //$"{Name,-15}";
    }
}