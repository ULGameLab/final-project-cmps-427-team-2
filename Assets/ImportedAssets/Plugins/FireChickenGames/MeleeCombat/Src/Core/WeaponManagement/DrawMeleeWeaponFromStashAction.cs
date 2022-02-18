namespace FireChickenGames.MeleeCombat.Core.WeaponManagement
{
    using FireChickenGames.Combat;
    using GameCreator.Core;
    using System.Collections;
    using UnityEngine;

    [AddComponentMenu("")]
    public class DrawMeleeWeaponFromStashAction : IAction
    {
        [Tooltip("A game object with a weapon stash component.")]
        public TargetGameObject weaponWielder = new TargetGameObject(TargetGameObject.Target.Player);

        protected WeaponStash weaponStash;

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            if (weaponWielder == null)
                yield break;

            var weaponStash = weaponWielder.GetComponent<WeaponStash>(target);
            if (weaponStash != null)
                yield return weaponStash.DrawMelee();
        }

#if UNITY_EDITOR
        public static new string NAME = "Fire Chicken Games/Combat/Draw Melee Weapon From Stash";
        private const string NODE_TITLE = "{0} draw current melee weapon";

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
