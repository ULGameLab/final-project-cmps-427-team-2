namespace FireChickenGames.Combat.Core.Attacking
{
	using FireChickenGames.Combat;
    using UnityEngine;
	using GameCreator.Core;

#if UNITY_EDITOR
	using UnityEditor;
#endif

    [AddComponentMenu("")]
	public class AttackWithEquippedWeaponAction : IAction
	{
		[Tooltip("A game object with a weapon stash component.")]
		public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.Player);

		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
			if (targetGameObject == null)
				return false;
			var weaponStash = targetGameObject.GetComponent<WeaponStash>(target);
			if (weaponStash != null)
			{
				weaponStash.StartChargedShot();
				StartCoroutine(weaponStash.Attack());
			}
			return true;
        }

		#if UNITY_EDITOR
		public static new string NAME = "Fire Chicken Games/Combat/Attack With Equipped Weapon";
		private SerializedProperty spTargetGameObject;

		public override string GetNodeTitle()
		{
			return "Attack With Equipped Weapon";
		}

		protected override void OnEnableEditorChild()
		{
			spTargetGameObject = serializedObject.FindProperty("targetGameObject");
		}

		protected override void OnDisableEditorChild()
		{
			spTargetGameObject = null;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(spTargetGameObject, new GUIContent("Target"));
			serializedObject.ApplyModifiedProperties();
		}
		#endif
	}
}
