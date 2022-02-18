namespace FireChickenGames.Combat.Editor
{
    using GameCreator.Core;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(WeaponStash))]
    public class WeaponStashEditor : Editor
    {
        protected WeaponStash instance;

        public SerializedProperty character;
        public SerializedProperty weapons;
        public SerializedProperty equippedWeapon;
        public SerializedProperty weaponStashUi;

        protected void OnEnable()
        {
            instance = target as WeaponStash;

            character = serializedObject.FindProperty("character");
            weapons = serializedObject.FindProperty("weapons");
            equippedWeapon = serializedObject.FindProperty("equippedWeapon");
            weaponStashUi = serializedObject.FindProperty("weaponStashUi");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(character, new GUIContent("Target"));
            EditorGUILayout.PropertyField(weaponStashUi, new GUIContent("UI"));

            EditorGUI.BeginDisabledGroup(true);

            EditorGUILayout.PropertyField(equippedWeapon);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(weapons);

            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
