namespace FireChickenGames.Combat.Core.Aiming
{
    using GameCreator.Core;
    using UnityEngine;

	[AddComponentMenu("")]
	public class IsAimingAtCurrentTargetCondition : ICondition
	{
		[Tooltip("A game object with a Targeter component.")]
		public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.Player);

		public override bool Check(GameObject target)
		{
			if (targetGameObject == null)
				return false;

			var targeter = targetGameObject.GetComponent<Targeter>(target);
			return targeter != null && targeter.IsAimingAtTarget();
		}
        
		#if UNITY_EDITOR
        public static new string NAME = "Fire Chicken Games/Combat/Is Aiming At Current Target";
		private const string NODE_TITLE = "Is {0} Aiming At Current Target";

		public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE, targetGameObject.target);
		}
		#endif
	}
}
