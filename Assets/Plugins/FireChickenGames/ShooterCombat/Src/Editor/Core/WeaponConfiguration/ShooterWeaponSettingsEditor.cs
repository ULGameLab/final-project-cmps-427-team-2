namespace FireChickenGames.ShooterCombat.Editor.Core.WeaponConfiguration
{
    using FireChickenGames.ShooterCombat.Core.WeaponConfiguration;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(ShooterWeaponSettings))]
    public class ShooterWeaponSettingsEditor : Editor
    {
        protected ShooterWeaponSettings instance;

        public SerializedProperty shouldOverrideAmmoSettingsFireModeNames;
        public SerializedProperty fireModeNameNone;
        public SerializedProperty fireModeNameSingle;
        public SerializedProperty fireModeNameBurst;
        public SerializedProperty fireModeNameAuto;

        public SerializedProperty ammoSettings;

        protected void OnEnable()
        {
            instance = target as ShooterWeaponSettings;

            ammoSettings = serializedObject.FindProperty("ammoSettings");

            shouldOverrideAmmoSettingsFireModeNames = serializedObject.FindProperty("shouldOverrideAmmoSettingsFireModeNames");
            fireModeNameNone = serializedObject.FindProperty("fireModeNameNone");
            fireModeNameSingle = serializedObject.FindProperty("fireModeNameSingle");
            fireModeNameBurst = serializedObject.FindProperty("fireModeNameBurst");
            fireModeNameAuto = serializedObject.FindProperty("fireModeNameAuto");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(shouldOverrideAmmoSettingsFireModeNames, new GUIContent("Override Display Names"));

            EditorGUI.BeginDisabledGroup(!shouldOverrideAmmoSettingsFireModeNames.boolValue);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(fireModeNameSingle, new GUIContent("Single Fire"));
            EditorGUILayout.PropertyField(fireModeNameBurst, new GUIContent("Burst Fire"));
            EditorGUILayout.PropertyField(fireModeNameAuto, new GUIContent("Auto Fire"));
            EditorGUI.indentLevel--;
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(ammoSettings, new GUIContent("Ammo Settings"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
