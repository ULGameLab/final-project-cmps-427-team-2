namespace FireChickenGames.MeleeCombat.Editor.Core.WeaponConfiguration
{
    using FireChickenGames.MeleeCombat.Core.WeaponConfiguration;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(MeleeWeaponSettings))]
    public class MeleeWeaponSettingsEditor : Editor
    {
        protected MeleeWeaponSettings instance;

        public SerializedProperty initialMeleeActionKey;

        protected void OnEnable()
        {
            instance = target as MeleeWeaponSettings;

            initialMeleeActionKey = serializedObject.FindProperty("initialMeleeActionKey");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(initialMeleeActionKey, new GUIContent("Initial Melee Action"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
