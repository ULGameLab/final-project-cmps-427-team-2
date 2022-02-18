namespace FireChickenGames.ShooterCombat.Core.WeaponManagement
{
    using System.Collections;
    using FireChickenGames.Combat;
    using FireChickenGames.ShooterCombat.Core.WeaponConfiguration;
    using GameCreator.Core;
    using GameCreator.Shooter;
    using UnityEngine;

    [AddComponentMenu("")]
	public class GiveShooterWeaponAction : IAction
	{
        [Tooltip("A game object with a weapon stash component.")]
        public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.Player);
        [Tooltip("A Shooter module weapon.")]
        public Weapon weapon;
        [Tooltip("A Shooter module ammo appropriate for the weapon.")]
        public Ammo ammo;
        [Tooltip("A an optional configuration object for Shooter weapons.")]
        public ShooterWeaponSettings shooterWeaponSettings;

        protected WeaponStash weaponStash; 

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            if (targetGameObject == null)
                yield break;

            weaponStash = targetGameObject.GetComponent<WeaponStash>(target);

            if (weaponStash != null)
                yield return weaponStash.GiveWeapon(weapon, ammo, shooterWeaponSettings);
        }

        #if UNITY_EDITOR
        public static new string NAME = "Fire Chicken Games/Combat/Give Shooter Weapon";
        private const string NODE_TITLE = "Give {0} {1} to {2} {3}";

        public override string GetNodeTitle()
        {
            return string.Format(
                NODE_TITLE,
                weapon == null ? "(none)" : weapon.name,
                ammo == null ? "" : $"with {ammo.name}",
                targetGameObject == null ? "(no one)" : targetGameObject.ToString(),
                shooterWeaponSettings == null ? "" : $"configured with {shooterWeaponSettings.name} settings"
            );;
        }
        #endif
    }
}
