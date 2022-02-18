namespace FireChickenGames.Combat.Core.WeaponManagement
{
    using FireChickenGames.MeleeCombat.Core.WeaponConfiguration;
    using GameCreator.Core;
    using GameCreator.Melee;
    using System.Collections;
    using UnityEngine;

    [AddComponentMenu("")]
    public class GiveMeleeWeaponAction : IAction
    {
        [Tooltip("A game object with a weapon stash component.")]
        public TargetGameObject weaponWielder = new TargetGameObject(TargetGameObject.Target.Player);
        public MeleeWeapon meleeWeapon;
        [Tooltip("A an optional configuration object for Melee weapons.")]
        public MeleeWeaponSettings meleeWeaponSettings;

        protected WeaponStash weaponStash;

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            if (weaponWielder == null)
                yield break;

            weaponStash = weaponWielder.GetComponent<WeaponStash>(target);
            if (weaponStash != null)
                yield return weaponStash.GiveWeapon(meleeWeapon, null, meleeWeaponSettings);
        }

#if UNITY_EDITOR
        public static new string NAME = "Fire Chicken Games/Combat/Give Melee Weapon";
        private const string NODE_TITLE = "Give {0} to {1}";

        public override string GetNodeTitle()
        {
            return string.Format(
                NODE_TITLE,
                meleeWeapon == null ? "(none)" : meleeWeapon.name,
                weaponWielder == null ? "(no one)" : weaponWielder.ToString()
            );
        }
#endif
    }
}
