namespace PokeD.BattleEngine.Attack.Enums
{
    public enum Target
    {  
        SpecificMove            = 1,
        SelectedMontserMeFirst  = 2,
        Ally                    = 3,
        UsersField              = 4,
        UserOrAlly              = 5,
        OpponentsField          = 6,
        User                    = 7,
        RandomOpponent          = 8,
        AllOtherMonster         = 9,
        SelectedMonster         = 10,
        AllOpponents            = 11,
        EntireField             = 12,
        UserAndAllies           = 13,
        AllMonster              = 14,

        /*
        Self,
        SelfAdjacentAllies,
        SelfNonAdjacentAllies,
        SelfAllAllies,

        AdjacentFoes,
        NonAdjacentFoes,
        AllFoes,
        */


        /*
        AllAdjacent,
        AllAdjacentFoes,
        All,
        AllFoes,
        AllAllies,
        */

        /*
        AdjacentFoes,
        AllFoes,
        AnyOther,
        AllAdjacent,
        All,
        Self,
        AdjacentAlly,
        WholeTeam
        */
    }
}