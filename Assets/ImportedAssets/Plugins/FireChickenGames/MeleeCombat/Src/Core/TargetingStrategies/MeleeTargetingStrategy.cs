namespace FireChickenGames.MeleeCombat.Core.TargetingStrategies
{
    using FireChickenGames.Combat.Core.Targeting;
    using FireChickenGames.Combat.Core.TargetingStrategies;
    using GameCreator.Melee;
    using UnityEngine;

    public class MeleeTargetingStrategy : TargetingStrategy
    {
        protected CharacterMelee characterMelee;

        public override void SetCombatCharacter(Component combatCharacter)
        {
            characterMelee = (CharacterMelee)combatCharacter;
        }

        public override void SetTargetFocus(ProximityTarget proximityTarget)
        {
            base.SetTargetFocus(proximityTarget);

            var targetGameObject = proximityTarget?.GameObject;
            if (targetGameObject == null)
                return;

            if (!targetGameObject.TryGetComponent(out TargetMelee targetMelee))
                targetMelee = targetGameObject.AddComponent<TargetMelee>();

            characterMelee?.SetTargetFocus(targetMelee);
        }

        public override void ReleaseTargetFocus()
        {
            base.ReleaseTargetFocus();
            if (characterMelee ?? false)
                characterMelee.ReleaseTargetFocus();
        }

        public override bool HasTargetFocus()
        {
            return characterMelee != null && characterMelee.HasFocusTarget;
        }
    }
}
