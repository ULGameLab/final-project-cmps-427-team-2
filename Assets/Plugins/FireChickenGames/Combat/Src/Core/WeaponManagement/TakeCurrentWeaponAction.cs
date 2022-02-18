namespace FireChickenGames.Combat.Core.WeaponManagement
{
    using GameCreator.Core;
    using System.Collections;
    using UnityEngine;

    [AddComponentMenu("")]
    public class TakeCurrentWeaponAction : IAction
    {
        [Tooltip("A game object with a weapon stash component.")]
        public TargetGameObject weaponWielder = new TargetGameObject(TargetGameObject.Target.Player);
        protected WeaponStash weaponStash;

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            if (weaponWielder == null)
                yield break;

            weaponStash = weaponWielder.GetComponent<WeaponStash>(target);
            if (weaponStash != null)
                yield return weaponStash.TakeCurrentWeapon();
        }

#if UNITY_EDITOR
        public static new string NAME = "Fire Chicken Games/Combat/Take Current Weapon";
        private const string NODE_TITLE = "Take current weapon from {0}";

        public override string GetNodeTitle()
        {
            return string.Format(
                NODE_TITLE,
                weaponWielder == null ? "(no one)" : weaponWielder.ToString()
            );
        }
#endif
    }
}
