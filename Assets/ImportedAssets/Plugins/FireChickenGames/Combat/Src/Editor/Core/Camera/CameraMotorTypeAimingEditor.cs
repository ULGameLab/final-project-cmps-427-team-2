namespace FireChickenGames.Combat.Editor.Core.Cameras
{
    using UnityEngine;
    using UnityEditor;
    using FireChickenGames.Combat.Core.Cameras;
    using GameCreator.Camera;

    [CustomEditor(typeof(CameraMotorTypeAiming))]
    public class CameraMotorTypeAimingEditor : ICameraMotorTypeEditor
    {
        private static readonly Color HANDLE_COLOR_BACKG = new Color(256f, 256f, 256f, 0.2f);
        private static readonly Color HANDLE_COLOR_PITCH_P = new Color(256f, 0f, 0f, 0.2f);
        private static readonly Color HANDLE_COLOR_PITCH_C = new Color(256f, 0f, 0f, 0.8f);
        private static readonly Color HANDLE_COLOR_RADIUS = new Color(256f, 256f, 0f, 0.1f);

        // Zoom.
        private SerializedProperty allowZoom;
        private SerializedProperty initialZoom;
        private SerializedProperty zoomSpeed;
        private SerializedProperty zoomSensitivity;
        private SerializedProperty zoomLimits;

        // Target.
        private SerializedProperty targetGameObject;
        private SerializedProperty targetOffset;
        private SerializedProperty pivotOffset;

        // Mouse orbit.
        private SerializedProperty allowOrbitInput;
        private SerializedProperty orbitInput;
        private SerializedProperty orbitSpeed;
        private SerializedProperty sensitivity;
        private SerializedProperty maxPitch;

        // Wall avoidance.
        private SerializedProperty avoidWallClip;
        private SerializedProperty wallClipRadius;
        private SerializedProperty wallClipLayerMask;

        protected override void OnSubEnable()
        {
            allowZoom = serializedObject.FindProperty("allowZoom");
            initialZoom = serializedObject.FindProperty("initialZoom");
            zoomSpeed = serializedObject.FindProperty("zoomSpeed");
            zoomSensitivity = serializedObject.FindProperty("zoomSensitivity");
            zoomLimits = serializedObject.FindProperty("zoomLimits");

            targetGameObject = serializedObject.FindProperty("target");
            targetOffset = serializedObject.FindProperty("targetOffset");
            pivotOffset = serializedObject.FindProperty("pivotOffset");
            allowOrbitInput = serializedObject.FindProperty("allowOrbitInput");
            orbitInput = serializedObject.FindProperty("orbitInput");
            orbitSpeed = serializedObject.FindProperty("orbitSpeed");
            sensitivity = serializedObject.FindProperty("sensitivity");
            maxPitch = serializedObject.FindProperty("maxPitch");

            avoidWallClip = serializedObject.FindProperty("avoidWallClip");
            wallClipRadius = serializedObject.FindProperty("wallClipRadius");
            wallClipLayerMask = serializedObject.FindProperty("wallClipLayerMask");
        }

        protected override bool OnSubInspectorGUI(Transform cameraMotorTransform)
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(targetGameObject, new GUIContent("Target"));
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(targetOffset);
            EditorGUILayout.PropertyField(pivotOffset);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(allowOrbitInput);

            EditorGUI.BeginDisabledGroup(!allowOrbitInput.boolValue);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(orbitInput);
            EditorGUILayout.PropertyField(orbitSpeed);
            EditorGUI.indentLevel--;
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(sensitivity);
            EditorGUILayout.PropertyField(maxPitch);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(allowZoom);

            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(initialZoom);
            EditorGUILayout.PropertyField(zoomSpeed);
            EditorGUILayout.PropertyField(zoomSensitivity);
            EditorGUILayout.PropertyField(zoomLimits);

            var limitX = zoomLimits.vector2Value.x;
            var limitY = zoomLimits.vector2Value.y;
            EditorGUILayout.MinMaxSlider("Zoom Limits", ref limitX, ref limitY, 0.0f, 20f);
            zoomLimits.vector2Value = new Vector2(limitX, limitY);
            EditorGUI.indentLevel--;

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

        // SCENE GUI: --------------------------------------------------------------------------------------------------

        public override bool OnSubSceneGUI(Transform cameraMotorTransform)
        {
            if (Application.isPlaying)
                return false;

            serializedObject.Update();

            Handles.color = HANDLE_COLOR_BACKG;
            Handles.DrawSolidArc(
                cameraMotorTransform.position,
                cameraMotorTransform.TransformDirection(Vector3.right),
                cameraMotorTransform.TransformDirection(Vector3.up),
                180f,
                HandleUtility.GetHandleSize(cameraMotorTransform.position)
            );

            var angle = maxPitch.floatValue;
            var direction = Quaternion.AngleAxis(-angle / 2.0f, Vector3.right) * Vector3.forward;

            Handles.color = HANDLE_COLOR_PITCH_P;
            Handles.DrawSolidArc(
                cameraMotorTransform.position,
                cameraMotorTransform.TransformDirection(Vector3.right),
                cameraMotorTransform.TransformDirection(direction),
                angle,
                HandleUtility.GetHandleSize(cameraMotorTransform.position)
            );

            Handles.color = HANDLE_COLOR_PITCH_C;
            Handles.DrawWireArc(
                cameraMotorTransform.position,
                cameraMotorTransform.TransformDirection(Vector3.right),
                cameraMotorTransform.TransformDirection(direction),
                angle,
                HandleUtility.GetHandleSize(cameraMotorTransform.position)
            );

            Handles.color = HANDLE_COLOR_RADIUS;
            Handles.DrawSolidArc(
                cameraMotorTransform.position,
                Vector3.up,
                Vector3.right,
                360f,
                wallClipRadius.floatValue
            );

            serializedObject.ApplyModifiedProperties();
            return false;
        }

        public override bool ShowPreviewCamera()
        {
            return false;
        }
    }
}