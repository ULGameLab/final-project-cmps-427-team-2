namespace FireChickenGames.Combat.Core.Aiming
{
    using System.Collections;
    using UnityEngine;
    using GameCreator.Core;

    [AddComponentMenu("")]
    public class StopAimingAtCurrentTargetAction : IAction
    {
        [Tooltip("A game object with a Targeter component.")]
        public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.Player);

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            if (targetGameObject == null)
                yield break;

            var targeter = targetGameObject.GetComponent<Targeter>(target);
            
            if (targeter != null)
                targeter.StopAimingAtTarget();

            yield return 0;
        }

#if UNITY_EDITOR
        public static new string NAME = "Fire Chicken Games/Combat/Stop Aiming At Current Target";
        private const string NODE_TITLE = "Stop Aiming At Current Target";

        public override string GetNodeTitle()
        {
            return NODE_TITLE;
        }
#endif
    }
}
