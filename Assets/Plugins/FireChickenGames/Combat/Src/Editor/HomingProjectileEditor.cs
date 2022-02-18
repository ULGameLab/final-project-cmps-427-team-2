namespace FireChickenGames.Combat.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(HomingProjectile))]
    public class HomingProjectileEditor : Editor
    {
        protected HomingProjectile instance;

        public SerializedProperty targetMode;
        public SerializedProperty targetGameObject;
        public SerializedProperty targeterGameObject;

        public SerializedProperty ammoRigidbody;
        public SerializedProperty targetOffset;
        public SerializedProperty secondsToWaitBeforePropelling;
        public SerializedProperty propelForPeriodInSeconds;
        public SerializedProperty maximumTurnAngle;
        public SerializedProperty velocity;

        protected void OnEnable()
        {
            instance = target as HomingProjectile;

            targetMode = serializedObject.FindProperty("targetMode");
            targetGameObject = serializedObject.FindProperty("targetGameObject");
            targeterGameObject = serializedObject.FindProperty("targeterGameObject");

            targetOffset = serializedObject.FindProperty("targetOffset");
            ammoRigidbody = serializedObject.FindProperty("ammoRigidbody");
            secondsToWaitBeforePropelling = serializedObject.FindProperty("secondsToWaitBeforePropelling");
            propelForPeriodInSeconds = serializedObject.FindProperty("propelForPeriodInSeconds");
            maximumTurnAngle = serializedObject.FindProperty("maximumTurnAngle");
            velocity = serializedObject.FindProperty("velocity");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(targetMode);
            if (instance.IsTargetingModeTargeter)
                EditorGUILayout.PropertyField(targeterGameObject, new GUIContent("Targeter"));
            if (instance.IsTargetingModeTarget)
                EditorGUILayout.PropertyField(targetGameObject, new GUIContent("Target"));

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(ammoRigidbody);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(targetOffset);
            EditorGUILayout.PropertyField(secondsToWaitBeforePropelling, new GUIContent("Propulsion Delay (seconds)"));
            EditorGUILayout.PropertyField(propelForPeriodInSeconds, new GUIContent("Propel For (seconds)"));
            EditorGUILayout.PropertyField(maximumTurnAngle);
            EditorGUILayout.PropertyField(velocity);

            EditorGUILayout.Space();

            var targeterName = instance.targeter ? instance.targeter.gameObject.name : "Not set";
            EditorGUILayout.HelpBox($"Targeter: {targeterName}", MessageType.Info);

            var targetName = instance.targetTransform ? instance.targetTransform.gameObject.name : "Not set";
            if (instance.targetTransform)
            EditorGUILayout.HelpBox($"Target: {targetName}", MessageType.Info);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
