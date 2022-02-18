namespace FireChickenGames.ShooterCombat.Core.Attacking
{
	using System.Collections;
	using UnityEngine;
	using GameCreator.Core;
	using GameCreator.Shooter;

#if UNITY_EDITOR
	using UnityEditor;
#endif

    [AddComponentMenu("")]
	public class HipFireAction : IAction
	{
		public TargetGameObject shooter = new TargetGameObject(TargetGameObject.Target.Player);
		[Range(0, 5)]
		public float preShootDelay = 0.1f;
		[Range(0, 5)]
		public float postShootDelay = 0.1f;
		private bool forceStop = false;

		// EXECUTABLE: ----------------------------------------------------------------------------

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
			yield return Shoot(target);
        }

		public IEnumerator Shoot(GameObject target)
        {
			this.forceStop = false;

			float startTime = Time.time + preShootDelay;
			var waitUntilBefore = new WaitUntil(() => Time.time > startTime || forceStop);
			float stopTime = Time.time + postShootDelay;
			var waitUntilAfter = new WaitUntil(() => Time.time > stopTime || forceStop);

			var characterShooter = shooter.GetComponent<CharacterShooter>(target);
			if (characterShooter != null && !characterShooter.isAiming)
			{
				characterShooter.StartAiming(new AimingCameraDirection(characterShooter));
				WeaponCrosshair.Destroy();
				yield return waitUntilBefore;
				characterShooter.Shoot();
				yield return waitUntilAfter;
				characterShooter.StopAiming();
			}

			yield return 0;
		}

		public override void Stop()
		{
			forceStop = true;
		}

		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

		public static new string NAME = "Fire Chicken Games/Combat/Shoot Without Aiming";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spShooter;
		private SerializedProperty spPreShootDelay;
		private SerializedProperty spPostShootDelay;

		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
			return "Fire a shooter weaping without aiming";
		}

		protected override void OnEnableEditorChild ()
		{
			spShooter = serializedObject.FindProperty("shooter");
			spPreShootDelay = serializedObject.FindProperty("preShootDelay");
			spPostShootDelay = serializedObject.FindProperty("postShootDelay");
		}

		protected override void OnDisableEditorChild ()
		{
			spShooter = null;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(spShooter, new GUIContent("Shooter"));
			EditorGUILayout.PropertyField(spPreShootDelay, new GUIContent("Delay Before Shooting (Seconds)"));
			EditorGUILayout.PropertyField(spPostShootDelay, new GUIContent("Delay After Shooting (Seconds)"));
			serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
