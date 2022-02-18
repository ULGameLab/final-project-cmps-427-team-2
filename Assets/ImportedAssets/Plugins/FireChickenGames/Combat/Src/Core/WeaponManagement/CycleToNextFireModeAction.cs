namespace FireChickenGames.Combat.Core.WeaponManagement
{
	using System.Collections;
	using UnityEngine;
	using GameCreator.Core;

    [AddComponentMenu("")]
	public class CycleToNextFireModeAction : IAction
	{
        [Tooltip("A game object with a weapon stash component.")]
        public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.Player);
        [Tooltip("Select fire mode in reverse order.")]
        public bool reverseSelection;

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            if (targetGameObject == null)
                yield break;

            var weaponStash = targetGameObject.GetComponent<WeaponStash>(target);
            if (weaponStash != null)
                yield return weaponStash.CycleToNextFireMode(reverseSelection);
        }

        #if UNITY_EDITOR
        public static new string NAME = "Fire Chicken Games/Combat/Cycle To Next Fire Mode";
        private const string NODE_TITLE = "Cycle To Next Fire Mode";

        public override string GetNodeTitle()
        {
            return NODE_TITLE;
        }
        #endif
    }
}
