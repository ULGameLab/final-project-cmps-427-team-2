namespace FireChickenGames.Combat.Editor.Core.Cameras
{
    using UnityEngine;
    using UnityEditor;
    using FireChickenGames.Combat.Core.Cameras;
    using GameCreator.Camera;

    [CustomEditor(typeof(CameraMotorTypeTargeting))]
    public class CameraMotorTypeTargetingEditor : ICameraMotorTypeEditor
    {
        // Anchor.
        private SerializedProperty anchorDistance;
        private SerializedProperty horizontalOffset;
        private SerializedProperty anchor;
        private SerializedProperty anchorOffset;

        // Wall avoidance.
        private SerializedProperty avoidWallClip;
        private SerializedProperty wallClipRadius;
        private SerializedProperty wallClipLayerMask;

        protected override void OnSubEnable()
        {
            anchorDistance = serializedObject.FindProperty("anchorDistance");
            horizontalOffset = serializedObject.FindProperty("horizontalOffset");
            anchor = serializedObject.FindProperty("anchor");
            anchorOffset = serializedObject.FindProperty("anchorOffset");

            avoidWallClip = serializedObject.FindProperty("avoidWallClip");
            wallClipRadius = serializedObject.FindProperty("wallClipRadius");
            wallClipLayerMask = serializedObject.FindProperty("wallClipLayerMask");
        }

        public override bool ShowPreviewCamera()
        {
            return false;
        }

        protected override bool OnSubInspectorGUI(Transform cameraMotorTransform)
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(anchorDistance);
            EditorGUILayout.PropertyField(horizontalOffset);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(anchor);
            EditorGUILayout.PropertyField(anchorOffset);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(avoidWallClip);

            EditorGUI.BeginDisabledGroup(!avoidWallClip.boolValue);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(wallClipRadius);
            EditorGUILayout.PropertyField(wallClipLayerMask);
            EditorGUI.indentLevel--;
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
            return false;
        }
    }
}
