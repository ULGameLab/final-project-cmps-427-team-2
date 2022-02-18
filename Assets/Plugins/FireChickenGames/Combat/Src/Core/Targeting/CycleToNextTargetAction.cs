namespace FireChickenGames.Combat.Core.Targeting
{
    using System.Collections;
    using UnityEngine;
    using GameCreator.Core;

    [AddComponentMenu("")]
    public class CycleToNextTargetAction : IAction
    {
        [Tooltip("A game object with a Targeter component.")]
        public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.Player);
        [Tooltip("Switch to previous target.")]
        public bool reverseSelection;

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            if (targetGameObject == null)
                yield break;

            var targeter = targetGameObject.GetComponent<Targeter>(target);
            if (targeter != null)
                targeter.CycleToNextTarget(reverseSelection);

            yield return 0;
        }

#if UNITY_EDITOR
        public static new string NAME = "Fire Chicken Games/Combat/Cycle To Next Target";
        private const string NODE_TITLE = "Cycle To Next Target";

        public override string GetNodeTitle()
        {
            return NODE_TITLE;
        }
#endif
    }
}
