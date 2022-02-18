namespace FireChickenGames.Combat.Core.Targeting
{
    using System.Collections;
    using UnityEngine;
    using GameCreator.Core;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class AcquireInitialTargetAction : IAction
    {
        [Tooltip("A game object with a Targeter component.")]
        public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.Player);

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            if (targetGameObject == null)
                yield break;

            var targeter = targetGameObject.GetComponent<Targeter>(target);
            
            if (targeter != null)
                targeter.AcquireInitialTarget();

            yield return 0;
        }

#if UNITY_EDITOR
        public static new string NAME = "Fire Chicken Games/Combat/Acquire Initial Target";
        private const string NODE_TITLE = "Acquire Initial Target";

        public override string GetNodeTitle()
        {
            return NODE_TITLE;
        }
#endif
    }
}
