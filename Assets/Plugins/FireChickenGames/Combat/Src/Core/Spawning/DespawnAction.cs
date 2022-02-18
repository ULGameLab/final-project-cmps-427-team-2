namespace FireChickenGames.Combat.Core.Spawning
{
	using System.Collections;
	using UnityEngine;
	using GameCreator.Core;

#if UNITY_EDITOR
	using UnityEditor;
#endif

	[AddComponentMenu("")]
	public class DespawnAction : IAction
	{
		public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.GameObject);

		// EXECUTABLE: ----------------------------------------------------------------------------

		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
		{
			if (targetGameObject == null)
				return false;
			var spawner = targetGameObject?.GetComponent<Spawner>(target);
			if (spawner == null)
				return false;
			spawner.Despawn();
			return true;
		}

		public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
		{
			if (targetGameObject == null)
				yield break;
			var spawner = targetGameObject?.GetComponent<Spawner>(target);
			if (spawner == null)
				yield break;

			spawner.Despawn();
			yield return base.Execute(target, actions, index);
		}

		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR
		public static new string NAME = "Fire Chicken Games/Combat/Despawn";
		private const string NODE_TITLE = "Despawn";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spTargetGameObject;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return string.Format(NODE_TITLE);
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
