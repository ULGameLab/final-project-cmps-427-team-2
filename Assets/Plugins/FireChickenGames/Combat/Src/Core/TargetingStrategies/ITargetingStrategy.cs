namespace FireChickenGames.Combat.Core.TargetingStrategies
{
    using FireChickenGames.Combat.Core.Targeting;
    using UnityEngine;

    public interface ITargetingStrategy
    {
        void SetCombatCharacter(Component combatCharacter);
        void SetTargetFocus(ProximityTarget proximityTarget);
        void ReleaseTargetFocus();
        bool HasTargetFocus();
    }
}
