namespace FireChickenGames.Combat.Core.WeaponManagement
{
	using System.Collections;
	using UnityEngine;
	using GameCreator.Core;

    [AddComponentMenu("")]
	public class CycleToNextAmmoAction : IAction
	{
        [Tooltip("A game object with a weapon stash component.")]
        public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.Player);
        [Tooltip("Select ammo in reverse order.")]
        public bool reverseSelection;

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            if (targetGameObject == null)
                yield break;

            var weaponStash = targetGameObject.GetComponent<WeaponStash>(target);
            if (weaponStash != null)
                yield return weaponStash.CycleToNextAmmo(reverseSelection);
        }

        #if UNITY_EDITOR
        public static new string NAME = "Fire Chicken Games/Combat/Cycle To Next Ammo";
        private const string NODE_TITLE = "Cycle To Next Ammo";

        public override string GetNodeTitle()
        {
            return NODE_TITLE;
        }
        #endif
    }
}
