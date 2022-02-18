namespace FireChickenGames.Combat.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(WeaponStashUi))]
    public class WeaponStashUiEditor : Editor
    {
        protected WeaponStashUi instance;

        public SerializedProperty weaponNameText;
        public SerializedProperty weaponDescriptionText;

        public SerializedProperty ammoNameText;
        public SerializedProperty ammoDescriptionText;

        public SerializedProperty ammoInClipText;
        public SerializedProperty ammoMaxClipText;
        public SerializedProperty ammoInStorageText;

        public SerializedProperty fireModeText;

        protected void OnEnable()
        {
            instance = target as WeaponStashUi;

            weaponNameText = serializedObject.FindProperty("weaponNameText");
            weaponDescriptionText = serializedObject.FindProperty("weaponDescriptionText");

            ammoNameText = serializedObject.FindProperty("ammoNameText");
            ammoDescriptionText = serializedObject.FindProperty("ammoDescriptionText");

            ammoInClipText = serializedObject.FindProperty("ammoInClipText");
            ammoMaxClipText = serializedObject.FindProperty("ammoMaxClipText");
            ammoInStorageText = serializedObject.FindProperty("ammoInStorageText");

            fireModeText = serializedObject.FindProperty("fireModeText");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(weaponNameText, new GUIContent("Name"));
            EditorGUILayout.PropertyField(weaponDescriptionText, new GUIContent("Description"));

            EditorGUILayout.PropertyField(ammoNameText, new GUIContent("Name"));
            EditorGUILayout.PropertyField(ammoDescriptionText, new GUIContent("Description"));

            EditorGUILayout.PropertyField(ammoInClipText, new GUIContent("Current Amount"));
            EditorGUILayout.PropertyField(ammoMaxClipText, new GUIContent("Capacity"));
            EditorGUILayout.PropertyField(ammoInStorageText, new GUIContent("Amount In Storage"));

            EditorGUILayout.PropertyField(fireModeText, new GUIContent("Fire Mode"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
