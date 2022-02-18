namespace FireChickenGames.ShooterCombat.Editor.Core.WeaponConfiguration
{
    using FireChickenGames.ShooterCombat.Core.WeaponConfiguration;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(AmmoSettings))]
    public class AmmoSettingsPropertyDrawer : PropertyDrawer
    {
        protected float linesInPropertyDrawer = 12f;
        protected float yPadding = 5f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);

            // Do not make child fields indented.
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 1;

            var lineHeight = position.height / linesInPropertyDrawer;

            var ammo = property.FindPropertyRelative("ammo");
            EditorGUI.PropertyField(
                new Rect(position.x, position.y, position.width, lineHeight),
                property.FindPropertyRelative("ammo")
            );

            EditorGUI.indentLevel++;

            var lineNumber = 0;

            EditorGUI.PropertyField(GetRectForLine(position, lineHeight, ++lineNumber), property.FindPropertyRelative("isAmmoAimable"), new GUIContent("Enable Aiming"));

            var hipFireTypeRect = GetRectForLine(position, lineHeight, ++lineNumber);
            EditorGUI.PropertyField(hipFireTypeRect, property.FindPropertyRelative("hipFireType"));

            var fireModesRect = GetRectForLine(position, lineHeight, ++lineNumber);
            EditorGUI.LabelField(fireModesRect, "Fire Modes");

            EditorGUI.indentLevel++;

            // Single
            var hasSingleShotMode = property.FindPropertyRelative("hasSingleShotMode");
            EditorGUI.PropertyField(GetRectForLine(position, lineHeight, ++lineNumber), hasSingleShotMode, new GUIContent("Single Fire"));

            EditorGUI.BeginDisabledGroup(!hasSingleShotMode.boolValue);
            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(
                GetRectForLine(position, lineHeight, ++lineNumber),
                property.FindPropertyRelative("fireModeNameSingle"),
                new GUIContent("Display Name")
            );
            EditorGUI.indentLevel--;
            EditorGUI.EndDisabledGroup();

            // Burst
            var hasBurstMode = property.FindPropertyRelative("hasBurstMode");
            EditorGUI.PropertyField(GetRectForLine(position, lineHeight, ++lineNumber), hasBurstMode, new GUIContent("Burst Fire"));

            EditorGUI.BeginDisabledGroup(!hasBurstMode.boolValue);
            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(
                GetRectForLine(position, lineHeight, ++lineNumber),
                property.FindPropertyRelative("fireModeNameBurst"),
                new GUIContent("Display Name")
            );
            EditorGUI.PropertyField(
                GetRectForLine(position, lineHeight, ++lineNumber),
                property.FindPropertyRelative("burstAmount"),
                new GUIContent("Amount")
            );
            EditorGUI.indentLevel--;
            EditorGUI.EndDisabledGroup();

            // Auto
            var hasFullAutoMode = property.FindPropertyRelative("hasFullAutoMode");
            EditorGUI.PropertyField(GetRectForLine(position, lineHeight, ++lineNumber), hasFullAutoMode, new GUIContent("Auto Fire"));

            EditorGUI.BeginDisabledGroup(!hasFullAutoMode.boolValue);
            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(
                GetRectForLine(position, lineHeight, ++lineNumber),
                property.FindPropertyRelative("fireModeNameAuto"),
                new GUIContent("Display Name")
            );
            EditorGUI.indentLevel--;
            EditorGUI.EndDisabledGroup();

            // Set indent to original value.
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        protected Rect GetRectForLine(Rect position, float lineHeight, int lineNumber)
        {
            return new Rect(
                position.x,
                position.y + (lineHeight * lineNumber) + yPadding,
                position.width,
                lineHeight
            );
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * (linesInPropertyDrawer);
        }
    }
}