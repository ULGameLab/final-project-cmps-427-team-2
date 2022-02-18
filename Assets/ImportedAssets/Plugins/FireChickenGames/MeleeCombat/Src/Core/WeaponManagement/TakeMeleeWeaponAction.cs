namespace FireChickenGames.Combat.Core.WeaponManagement
{
    using GameCreator.Core;
    using GameCreator.Melee;
    using System.Collections;
    using UnityEngine;

    [AddComponentMenu("")]
    public class TakeMeleeWeaponAction : IAction
    {
        [Tooltip("A game object with a weapon stash component.")]
        public TargetGameObject weaponWielder = new TargetGameObject(TargetGameObject.Target.Player);
        public MeleeWeapon meleeWeapon;

        protected WeaponStash weaponStash;

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            if (weaponWielder == null)
                yield break;

            weaponStash = weaponWielder.GetComponent<WeaponStash>(target);
            if (weaponStash != null)
                yield return weaponStash.TakeWeapon(meleeWeapon);
        }

#if UNITY_EDITOR
        public static new string NAME = "Fire Chicken Games/Combat/Take Melee Weapon";
        private const string NODE_TITLE = "Take {0} from {1}";

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
