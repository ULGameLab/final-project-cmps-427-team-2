namespace FireChickenGames.Combat.Core.Attacking
{
	using System.Collections;
	using UnityEngine;
	using GameCreator.Core;

#if UNITY_EDITOR
	using UnityEditor;
#endif

	[AddComponentMenu("")]
	public class StopAttackingWithEquippedWeaponAction : IAction
	{
		[Tooltip("A game object with a weapon stash component.")]
		public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.Player);

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
			if (targetGameObject == null)
				yield break;
			var weaponStash = targetGameObject.GetComponent<WeaponStash>(target);
			if (weaponStash != null)
			{
				//StopCoroutine(weaponStash.Attack());
				yield return weaponStash.StopAttacking();
			}
			yield return 0;
		}

#if UNITY_EDITOR
		public static new string NAME = "Fire Chicken Games/Combat/Stop Attacking With Equipped Weapon";
		private SerializedProperty spTargetGameObject;

		public override string GetNodeTitle()
		{
			return "Stop Attacking With Equipped Weapon";
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
