namespace FireChickenGames.Combat.Core.Targeting
{
    using System.Collections;
    using UnityEngine;
    using GameCreator.Core;

    [AddComponentMenu("")]
    public class SetTargetingEnabledAction : IAction
    {
        [Tooltip("A game object with a Targeter component.")]
        public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.Player);
        public bool isEnabled = true;

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            if (targetGameObject == null)
                yield break;

            var targeter = targetGameObject.GetComponent<Targeter>(target);
            if (targeter != null)
                targeter.SetTargetingEnabled(isEnabled);

            yield return 0;
        }

#if UNITY_EDITOR
        public static new string NAME = "Fire Chicken Games/Combat/Set Targeting Enabled";
        private const string NODE_TITLE = "Set Targeting Enabled";

        public override string GetNodeTitle()
        {
            return NODE_TITLE;
        }
#endif
    }
}
