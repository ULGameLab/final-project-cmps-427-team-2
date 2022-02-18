namespace FireChickenGames.Combat.Editor
{
    using System.Linq;
    using GameCreator.ModuleManager;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(Targeter))]
    public class TargeterEditor : Editor
    {
        protected Targeter instance;
        public string[] targetingTypeOptions = new string[0];

        public SerializedProperty targetGameObject;
        public SerializedProperty weaponStashGameObject;
        public SerializedProperty currentTargetVariable;
        
        // Aiming
        public SerializedProperty autoAimAtTarget;
        public SerializedProperty aimingCameraMotor;
        public SerializedProperty startAimingTransitionDuration;
        public SerializedProperty stopAimingTransitionDuration;

        // Targeting
        public SerializedProperty targetingType;
        public SerializedProperty aimAssistDeselectTargetTolerance;
        public SerializedProperty aimAssistAcquireTargetTimerCooldown;
        public SerializedProperty isTargetingEnabled;
        public SerializedProperty isAcquireInitialTargetEnabled;
        public SerializedProperty broadcastContinuingToBeTargetedInSeconds;

        public SerializedProperty targetingCameraMotor;
        public SerializedProperty startTargetingTransitionDuration;
        public SerializedProperty stopTargetingTransitionDuration;

        public SerializedProperty onlyTargetVisibleToCamera;
        public SerializedProperty onlyTargetNonOccluded;
        public SerializedProperty layersToIgnoreForVisibilityOcclusion;

        // Input
        public SerializedProperty useNativeInputControls;
        public SerializedProperty targetLockEnabledOnKeyUp;
        public SerializedProperty selectNextTargetOnKeyUp;
        public SerializedProperty selectPreviousTargetOnKeyUp;

        protected void OnEnable()
        {
            instance = target as Targeter;

            targetGameObject = serializedObject.FindProperty("targetGameObject");
            weaponStashGameObject  = serializedObject.FindProperty("weaponStashGameObject");
            currentTargetVariable = serializedObject.FindProperty("currentTargetVariable");

            // Aiming
            autoAimAtTarget = serializedObject.FindProperty("autoAimAtTarget");
            aimingCameraMotor = serializedObject.FindProperty("aimingCameraMotor");
            startAimingTransitionDuration = serializedObject.FindProperty("startAimingTransitionDuration");
            stopAimingTransitionDuration = serializedObject.FindProperty("stopAimingTransitionDuration");

            // Targeting
            targetingType = serializedObject.FindProperty("targetingType");
            aimAssistDeselectTargetTolerance = serializedObject.FindProperty("aimAssistDeselectTargetTolerance");
            aimAssistAcquireTargetTimerCooldown = serializedObject.FindProperty("aimAssistAcquireTargetTimerCooldown");
            isTargetingEnabled = serializedObject.FindProperty("isTargetingEnabled");
            isAcquireInitialTargetEnabled = serializedObject.FindProperty("isAcquireInitialTargetEnabled");
            broadcastContinuingToBeTargetedInSeconds = serializedObject.FindProperty("broadcastContinuingToBeTargetedInSeconds");

            targetingCameraMotor = serializedObject.FindProperty("targetingCameraMotor");
            startTargetingTransitionDuration = serializedObject.FindProperty("startTargetingTransitionDuration");
            stopTargetingTransitionDuration = serializedObject.FindProperty("stopTargetingTransitionDuration");

            onlyTargetVisibleToCamera = serializedObject.FindProperty("onlyTargetVisibleToCamera");
            onlyTargetNonOccluded = serializedObject.FindProperty("onlyTargetNonOccluded");
            layersToIgnoreForVisibilityOcclusion = serializedObject.FindProperty("layersToIgnoreForVisibilityOcclusion");

            // Input
            useNativeInputControls = serializedObject.FindProperty("useNativeInputControls");
            targetLockEnabledOnKeyUp = serializedObject.FindProperty("targetLockEnabledOnKeyUp");
            selectNextTargetOnKeyUp = serializedObject.FindProperty("selectNextTargetOnKeyUp");
            selectPreviousTargetOnKeyUp = serializedObject.FindProperty("selectPreviousTargetOnKeyUp");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var headerLabelHeight = GUILayout.Height(20.0f);
            
            EditorGUILayout.PropertyField(targetGameObject, new GUIContent("Targeter"));
            EditorGUILayout.PropertyField(weaponStashGameObject, new GUIContent("Weapon Stash"));
            EditorGUILayout.PropertyField(currentTargetVariable, new GUIContent("Current Target (optional)"));
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            // Aiming
            EditorGUILayout.LabelField("Aiming", EditorStyles.boldLabel, headerLabelHeight);
            EditorGUILayout.PropertyField(autoAimAtTarget);

            EditorGUILayout.PropertyField(aimingCameraMotor, new GUIContent("Camera Motor"));
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(startAimingTransitionDuration, new GUIContent("Start Aiming Transition Duration"));
            EditorGUILayout.PropertyField(stopAimingTransitionDuration, new GUIContent("Stop Aiming Transition Duration"));
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            // Targeting
            EditorGUILayout.LabelField("Targeting", EditorStyles.boldLabel, headerLabelHeight);
            EditorGUILayout.PropertyField(isTargetingEnabled, new GUIContent("Enable"));
            EditorGUILayout.PropertyField(isAcquireInitialTargetEnabled, new GUIContent("Acquire Initial Target"));
            EditorGUILayout.PropertyField(broadcastContinuingToBeTargetedInSeconds, new GUIContent("Continuing To Be Targeted Event Period (in seconds)"));
            EditorGUILayout.PropertyField(targetingCameraMotor, new GUIContent("Camera Motor"));
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(startTargetingTransitionDuration, new GUIContent("Start Targeting Transition Duration"));
            EditorGUILayout.PropertyField(stopTargetingTransitionDuration, new GUIContent("Stop Targeting Transition Duration"));
            EditorGUI.indentLevel--;

            EditorGUILayout.PropertyField(targetingType, new GUIContent("Type"));
            if (instance.IsAimAssistTargeter())
            {
                var isShooterCombatEnabled = ModuleManager.IsEnabled(
                    ModuleManager.GetModuleManifest("com.firechickengames.module.shootercombat").module
                );

                EditorGUI.indentLevel++;

                if (isShooterCombatEnabled)
                {
                    EditorGUILayout.PropertyField(aimAssistDeselectTargetTolerance, new GUIContent("Deselect Target Input Threshold"));
                    EditorGUILayout.PropertyField(aimAssistAcquireTargetTimerCooldown, new GUIContent("Acquire Target Delay (Seconds)"));
                }
                else
                {
                    var aimAssistDepsMessage = "The Shooter module and Combat (Shooter) integration module are both required for aim assist.";
                    EditorGUILayout.HelpBox(aimAssistDepsMessage, MessageType.None, false);
                }

                EditorGUI.indentLevel--;
            }
            else if (instance.IsProximityTargeter())
            {
                EditorGUI.indentLevel++;

                // Target Visibility
                EditorGUILayout.PropertyField(onlyTargetVisibleToCamera, new GUIContent("Target Visible To Camera"));
                EditorGUILayout.PropertyField(onlyTargetNonOccluded, new GUIContent("Target Non-Occluded"));
                EditorGUI.indentLevel++;
                GUI.enabled = onlyTargetNonOccluded.boolValue;
                EditorGUILayout.PropertyField(layersToIgnoreForVisibilityOcclusion);
                GUI.enabled = true;
                EditorGUI.indentLevel--;

                // Input
                EditorGUILayout.PropertyField(useNativeInputControls, new GUIContent("Enable User Input"));
                EditorGUI.indentLevel++;
                GUI.enabled = useNativeInputControls.boolValue;
                EditorGUILayout.PropertyField(targetLockEnabledOnKeyUp, new GUIContent("Toggle Targeting On/Off"));
                EditorGUILayout.PropertyField(selectNextTargetOnKeyUp, new GUIContent("Select Next Target"));
                EditorGUILayout.PropertyField(selectPreviousTargetOnKeyUp, new GUIContent("Select Previous Target"));
                GUI.enabled = true;
                EditorGUI.indentLevel--;

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();



            serializedObject.ApplyModifiedProperties();
        }
    }
}
