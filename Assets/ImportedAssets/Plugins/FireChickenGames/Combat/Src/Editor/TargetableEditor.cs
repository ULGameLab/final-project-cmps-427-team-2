namespace FireChickenGames.Combat.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(Targetable))]
    public class TargetableEditor : Editor
    {
        protected Targetable instance;
        public SerializedProperty player;
        public SerializedProperty canBeTargeted;
        public SerializedProperty visibilityDetectionRenderer;

        public SerializedProperty showIndicator;
        public SerializedProperty indicatorPrefab;
        public SerializedProperty indicatorOffset;
        public SerializedProperty indicatorTextContent;
        public SerializedProperty indicatorTextColor;

        public SerializedProperty isMouseTargetSelectionEnabled;

        public SerializedProperty showMouseHoverIndicator;
        public SerializedProperty mouseHoverIndicatorPrefab;
        public SerializedProperty mouseHoverIndicatorOffset;
        public SerializedProperty mouseHoverIndicatorTextContent;
        public SerializedProperty mouseHoverIndicatorTextColor;

        public SerializedProperty onBecomeActiveTargetActions;
        public SerializedProperty onNotActiveTargetActions;
        public SerializedProperty onContinuingToBeTargetedActions;

        protected void OnEnable()
        {
            instance = target as Targetable;

            canBeTargeted = serializedObject.FindProperty("canBeTargeted");
            visibilityDetectionRenderer = serializedObject.FindProperty("visibilityDetectionRenderer");

            showIndicator = serializedObject.FindProperty("showIndicator");
            indicatorPrefab = serializedObject.FindProperty("indicatorPrefab");
            indicatorOffset = serializedObject.FindProperty("indicatorOffset");
            indicatorTextContent = serializedObject.FindProperty("indicatorTextContent");
            indicatorTextColor = serializedObject.FindProperty("indicatorTextColor");

            isMouseTargetSelectionEnabled = serializedObject.FindProperty("isMouseTargetSelectionEnabled");

            showMouseHoverIndicator = serializedObject.FindProperty("showMouseHoverIndicator");
            mouseHoverIndicatorPrefab = serializedObject.FindProperty("mouseHoverIndicatorPrefab");
            mouseHoverIndicatorOffset = serializedObject.FindProperty("mouseHoverIndicatorOffset");
            mouseHoverIndicatorTextContent = serializedObject.FindProperty("mouseHoverIndicatorTextContent");
            mouseHoverIndicatorTextColor = serializedObject.FindProperty("mouseHoverIndicatorTextColor");

            onBecomeActiveTargetActions = serializedObject.FindProperty("onBecomeActiveTargetActions");
            onNotActiveTargetActions = serializedObject.FindProperty("onNotActiveTargetActions");
            onContinuingToBeTargetedActions = serializedObject.FindProperty("onContinuingToBeTargetedActions");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(canBeTargeted);
            EditorGUILayout.PropertyField(visibilityDetectionRenderer);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(showIndicator, new GUIContent("Enabled"));
            EditorGUILayout.PropertyField(indicatorPrefab, new GUIContent("Prefab"));
            EditorGUILayout.PropertyField(indicatorOffset, new GUIContent("Offset"));
            EditorGUILayout.PropertyField(indicatorTextContent, new GUIContent("Text Content"));
            EditorGUILayout.PropertyField(indicatorTextColor, new GUIContent("Text Color"));

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(isMouseTargetSelectionEnabled, new GUIContent("Enabled"));

            EditorGUILayout.PropertyField(showMouseHoverIndicator, new GUIContent("Enabled"));
            EditorGUILayout.PropertyField(mouseHoverIndicatorPrefab, new GUIContent("Prefab"));
            EditorGUILayout.PropertyField(mouseHoverIndicatorOffset, new GUIContent("Offset"));
            EditorGUILayout.PropertyField(mouseHoverIndicatorTextContent, new GUIContent("Text Content"));
            EditorGUILayout.PropertyField(mouseHoverIndicatorTextColor, new GUIContent("Text Color"));

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(onBecomeActiveTargetActions, new GUIContent("On Become Active Target"));
            EditorGUILayout.PropertyField(onNotActiveTargetActions, new GUIContent("On Not Active Target"));
            EditorGUILayout.PropertyField(onContinuingToBeTargetedActions, new GUIContent("On Continuing To Be Targeted"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
