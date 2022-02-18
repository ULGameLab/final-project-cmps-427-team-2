namespace FireChickenGames.Combat.Core.Integrations
{
    using FireChickenGames.Combat.Core.TargetingStrategies;
    using GameCreator.Core;
    using System;
    using UnityEngine;

    public class MeleeIntegrationManager
    {
        public static bool IsMeleeModuleEnabled()
        {
            return GetMeleeTargetingStrategyType() != null;
        }

        public static Type GetCharacterMeleeType()
        {
            return Type.GetType("GameCreator.Melee.CharacterMelee");
        }

        public static Type GetMeleeTargetingStrategyType()
        {
            return Type.GetType("FireChickenGames.MeleeCombat.Core.TargetingStrategies.MeleeTargetingStrategy");
        }

        public static ITargetingStrategy MakeMeleeTargetingStrategyOrDefault(GameObject gameObject)
        {
            if (!IsMeleeModuleEnabled())
                return new TargetingStrategy();

            var characterMeleeType = GetCharacterMeleeType();
            if (characterMeleeType == null)
                return new TargetingStrategy();

            var hasCharacterMeleeComponent = gameObject.TryGetComponent(characterMeleeType, out var characterMelee);
            if (!hasCharacterMeleeComponent)
                return new TargetingStrategy();
            
            var meleeTargetingStrategyType = GetMeleeTargetingStrategyType();
            var targetingStrategy = Activator.CreateInstance(meleeTargetingStrategyType, characterMelee) as ITargetingStrategy;
            targetingStrategy.SetCombatCharacter(characterMelee);
            return targetingStrategy;
        }

        public static ICharacterMelee MakeCharacterMelee(TargetGameObject character, GameObject gameObject)
        {
            if (!IsMeleeModuleEnabled())
                return null;

            var characterMeleeAdapaterType = Type.GetType("FireChickenGames.MeleeCombat.Core.Integrations.CharacterMeleeAdapter");

            var characterMeleeAdapter = Activator.CreateInstance(characterMeleeAdapaterType) as ICharacterMelee;
            var characterMelee = character.GetComponent(gameObject, "GameCreator.Melee.CharacterMelee") as Component;
            characterMeleeAdapter.SetCharacterMelee(characterMelee);
            return characterMeleeAdapter;
        }
    }
}
