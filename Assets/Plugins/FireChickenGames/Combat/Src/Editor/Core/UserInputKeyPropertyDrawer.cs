namespace FireChickenGames.Combat.Editor.Core
{
    using FireChickenGames.Combat.Core.PlayerInput;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(UserInputKey))]
    public class UserInputKeyPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            var useMouseButton = property.FindPropertyRelative("useMouseButton");
            var mouseButton = property.FindPropertyRelative("mouseButton");
            var keyCode = property.FindPropertyRelative("keyCode");

            // Render the property drawer's label.
            position = EditorGUI.PrefixLabel(position, label);

            // Do not make child fields indented.
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var checkboxWidth = 20f;
            var useMouseButtonRect = new Rect(
                position.x + (position.width * 0.6f),
                position.y,
                checkboxWidth,
                position.height
            );

            var useMouseButtonLabelRect = new Rect(
                position.x + (position.width * 0.6f) + checkboxWidth,
                position.y,
                (position.width * 0.4f) - checkboxWidth,
                position.height
            );

            var keySelectRect = new Rect(
                position.x,
                position.y,
                position.width * 0.5f,
                position.height
            );

            EditorGUI.PropertyField(useMouseButtonRect, useMouseButton, GUIContent.none);
            EditorGUI.LabelField(useMouseButtonLabelRect, new GUIContent("Mouse Button"));
            EditorGUI.PropertyField(keySelectRect, useMouseButton.boolValue ? mouseButton : keyCode, GUIContent.none);

            // Set indent to original value.
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}