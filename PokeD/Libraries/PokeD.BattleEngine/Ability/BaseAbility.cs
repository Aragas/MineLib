namespace PokeD.BattleEngine.Ability
{
    public class BaseAbility
    {
        public IAbilityStaticData StaticData { get; }

        public bool IsHidden { get; }
        
        public BaseAbility(IAbilityStaticData staticData, bool isHidden) { StaticData = staticData; IsHidden = isHidden; }

        public override string ToString() => $"{StaticData}{(IsHidden ? " (H)" : string.Empty)}";
    }
}
