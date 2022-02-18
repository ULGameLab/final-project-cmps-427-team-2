namespace FireChickenGames.Combat.Core.Targeting
{
    using System.Collections;
    using UnityEngine;
    using GameCreator.Core;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class SetCurrentTargetAction : IAction
    {
        [Tooltip("A game object with a Targeter component.")]
        public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.Player);
        [Tooltip("GameObject with a Targetable component.")]
        [VariableFilter(Variable.DataType.GameObject)]
        public VariableProperty currentTargetVariable = new VariableProperty(Variable.VarType.GlobalVariable);

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            if (targetGameObject == null || currentTargetVariable.Get() == null)
                yield break;

            var targeter = targetGameObject.GetComponent<Targeter>(target);
            
            if (targeter != null)
                targeter.SetCurrentTarget(currentTargetVariable.Get() as GameObject);

            yield return 0;
        }

#if UNITY_EDITOR
        public static new string NAME = "Fire Chicken Games/Combat/Set Current Target";
        private const string NODE_TITLE = "Set Current Target";

        public override string GetNodeTitle()
        {
            return NODE_TITLE;
        }
#endif
    }
}
