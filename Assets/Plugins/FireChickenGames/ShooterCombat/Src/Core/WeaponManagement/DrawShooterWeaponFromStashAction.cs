namespace FireChickenGames.Combat.Core.WeaponManagement
{
    using System.Collections;
    using UnityEngine;
    using GameCreator.Core;

    [AddComponentMenu("")]
    public class DrawShooterWeaponFromStashAction : IAction
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
                yield return weaponStash.DrawShooter();
        }

        #if UNITY_EDITOR
        public static new string NAME = "Fire Chicken Games/Combat/Draw Shooter Weapon From Stash";
        private const string NODE_TITLE = "{0} draw current shooter weapon";

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
