namespace FireChickenGames.Combat.Core.TargetingStrategies
{
    using FireChickenGames.Combat.Core.Targeting;
    using UnityEngine;

    public class TargetingStrategy : ITargetingStrategy
    {
        protected Component _combatCharacter;

        public virtual void SetCombatCharacter(Component combatCharacter)
        {
            _combatCharacter = combatCharacter;
        }

        public virtual void ReleaseTargetFocus()
        {}

        public virtual void SetTargetFocus(ProximityTarget proximityTarget)
        {}

        public virtual bool HasTargetFocus()
        {
            return false;
        }
    }
}
