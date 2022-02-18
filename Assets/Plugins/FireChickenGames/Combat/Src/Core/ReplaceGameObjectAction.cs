namespace FireChickenGames.Combat.Core
{
	using System.Collections;
	using UnityEngine;
	using GameCreator.Core;

	#if UNITY_EDITOR
	using UnityEditor;
	#endif

	[AddComponentMenu("")]
	public class ReplaceGameObjectAction : IAction
	{
        public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.Invoker);
        public GameObject gameObjectToInstantiate;

		// EXECUTABLE: ----------------------------------------------------------------------------

		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
			var gameObjectToDestroy = targetGameObject.GetGameObject(target);

			if (gameObjectToInstantiate != null)
				Instantiate(gameObjectToInstantiate, gameObjectToDestroy.transform.position, gameObjectToDestroy.transform.rotation);

			Destroy(gameObjectToDestroy);
			return true;
        }

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
			var gameObjectToDestroy = targetGameObject.GetGameObject(target);

			if (gameObjectToInstantiate != null)
				Instantiate(gameObjectToInstantiate, gameObjectToDestroy.transform.position, gameObjectToDestroy.transform.rotation);

			Destroy(gameObjectToDestroy);
			return base.Execute(target, actions, index);
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR

        public static new string NAME = "Fire Chicken Games/Combat/Replace Game Object";
		private const string NODE_TITLE = "Replace target with a prefab.";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spTargetGameObject;
		private SerializedProperty spGameObjectToInstantiate;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
            return string.Format(NODE_TITLE);
        }

		protected override void OnEnableEditorChild ()
		{
			spTargetGameObject = serializedObject.FindProperty("targetGameObject");
			spGameObjectToInstantiate = serializedObject.FindProperty("gameObjectToInstantiate");
		}

		protected override void OnDisableEditorChild ()
		{
			spTargetGameObject = null;
			spGameObjectToInstantiate = null;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(spTargetGameObject, new GUIContent("Target"));
			EditorGUILayout.PropertyField(spGameObjectToInstantiate, new GUIContent("Replacement Game Object"));

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
