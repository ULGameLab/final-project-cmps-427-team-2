namespace FireChickenGames.Combat.Core.Integrations
{
    using FireChickenGames.Combat.Core.Aiming;
    using GameCreator.Core;
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    public class ShooterIntegrationManager
    {
        public static bool IsShooterModuleEnabled()
        {
            return GetAimingAtProximityTargetType() != null;
        }

        public static Type GetCharacterShooterType()
        {
            return Type.GetType("GameCreator.Shooter.CharacterShooter");
        }

        public static Type GetAimingAtProximityTargetType()
        {
            return Type.GetType("FireChickenGames.ShooterCombat.Core.Aiming.AimingAtProximityTarget");
        }

        public static IAimingAtProximityTarget MakeAimingAtProximityTargetOrDefault(ICharacterShooter characterShooter)
        {
            if (!IsShooterModuleEnabled() || characterShooter == null || !characterShooter.HasCharacterShooter())
                return new FocusOnTarget();

            var characterShooterType = GetCharacterShooterType();
            if (characterShooterType == null)
                return new FocusOnTarget();
            
            var aimingAtProximityTargetType = GetAimingAtProximityTargetType();
            var aimingAtProximityTarget = Activator.CreateInstance(aimingAtProximityTargetType, characterShooter);
            return aimingAtProximityTarget as IAimingAtProximityTarget;
        }

        public static ICharacterShooter MakeCharacterShooter(TargetGameObject character, GameObject gameObject, UnityAction<ScriptableObject> onChangeShooterWeapon = null, WeaponStashUi weaponStashUi = null)
        {
            if (!IsShooterModuleEnabled())
                return null;

            var characterShooterAdapterType = Type.GetType("FireChickenGames.ShooterCombat.Core.Integrations.CharacterShooterAdapter");

            var characterShooterAdapter = Activator.CreateInstance(characterShooterAdapterType) as ICharacterShooter;
            var characterShooter = character.GetComponent(gameObject, "GameCreator.Shooter.CharacterShooter") as Component;
            if (characterShooter == null)
                return null;
            characterShooterAdapter.SetCharacterShooter(characterShooter);
            characterShooterAdapter.SetStashedWeapon = onChangeShooterWeapon;
            characterShooterAdapter.WeaponStashUi = weaponStashUi;
            return characterShooterAdapter;
        }
    }
}
