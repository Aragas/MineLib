using System;

using PokeD.Core.Data.PokeApi;

namespace PokeD.Core.Test
{
    public static class PokeApiV2Class
    {
        private const int MaxPokemon            = 721;
        private const int MaxType               = 18;
        private const int MaxGender             = 3;
        private const int MaxAbility            = 190;
        private const int MaxEgggroup           = 15;
        private const int MaxItem               = 749;
        private const int MaxMove               = 621;
        private const int MaxEvolutionTrigger   = 4;


        public static void GetPokemon() { /* PokeApiV2.GetPokemon(new ResourceUri($"api/v2/pokemon/{new Random().Next(1, MaxPokemon)}", true)).Wait(); */ }
        public static void GetPokemonSpecies() { /* PokeApiV2.GetPokemonSpecies(new ResourceUri($"api/v2/pokemon-species/{new Random().Next(1, MaxPokemon)}", true)).Wait(); */ }
        public static void GetTypes() { /* PokeApiV2.GetTypes(new ResourceUri($"api/v2/type/{new Random().Next(1, MaxType)}", true)).Wait(); */ }
        public static void GetGender() { /* PokeApiV2.GetGender(new ResourceUri($"api/v2/gender/{new Random().Next(1, MaxGender)}", true)).Wait(); */ }
        public static void GetAbilities() { /* PokeApiV2.GetAbilities(new ResourceUri($"api/v2/ability/{new Random().Next(1, MaxAbility)}", true)).Wait(); */ }
        public static void GetEggGroups() { /* PokeApiV2.GetEggGroups(new ResourceUri($"api/v2/egg-group/{new Random().Next(1, MaxEgggroup)}", true)).Wait(); */ }
        public static void GetItems() { /* PokeApiV2.GetItems(new ResourceUri($"api/v2/item/{new Random().Next(1, MaxItem)}", true)).Wait(); */ }
        public static void GetMoves() { /* PokeApiV2.GetMoves(new ResourceUri($"api/v2/move/{new Random().Next(1, MaxMove)}", true)).Wait(); */ }
        public static void GetEvolutionTriggers() { /* PokeApiV2.GetEvolutionTriggers(new ResourceUri($"api/v2/evolution-trigger/{new Random().Next(1, MaxEvolutionTrigger)}", true)).Wait(); */ }
    }
}
