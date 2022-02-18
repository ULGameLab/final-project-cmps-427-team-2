namespace FireChickenGames.ShooterCombat.Core.WeaponManagement
{
    using System.Collections;
    using FireChickenGames.Combat;
    using GameCreator.Core;
    using GameCreator.Shooter;
    using UnityEngine;

    [AddComponentMenu("")]
	public class TakeShooterWeaponAction : IAction
	{
        [Tooltip("A game object with a weapon stash component.")]
        public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.Player);
        [Tooltip("A Shooter module weapon.")]
        public Weapon weapon;

        protected WeaponStash weaponStash; 

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            if (targetGameObject == null)
                yield break;

            weaponStash = targetGameObject.GetComponent<WeaponStash>(target);
            if (weaponStash != null)
                yield return weaponStash.TakeWeapon(weapon);
        }

        #if UNITY_EDITOR
        public static new string NAME = "Fire Chicken Games/Combat/Take Shooter Weapon";
        private const string NODE_TITLE = "Take {0} from {1}";

        public override string GetNodeTitle()
        {
            return string.Format(
                NODE_TITLE,
                weapon == null ? "(none)" : weapon.name,
                targetGameObject == null ?  "(no one)" : targetGameObject.ToString()
            );
        }
        #endif
    }
}
