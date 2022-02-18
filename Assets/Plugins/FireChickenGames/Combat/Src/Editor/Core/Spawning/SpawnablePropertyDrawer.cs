namespace FireChickenGames.Combat.Editor.Core.Spawning
{
    using FireChickenGames.Combat.Core.Spawning;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(SpawnPattern))]
    public class SpawnPatternPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            EditorGUI.BeginProperty(_position, GUIContent.none, _property);

            var spPatternType = _property.FindPropertyRelative("patternType");
            EditorGUI.PropertyField(_position, spPatternType);

            var patternType = (SpawnPattern.PatternType)spPatternType.intValue;

            if (patternType == SpawnPattern.PatternType.Default)
            {
                EditorGUI.indentLevel++;
                var spPoissonDiskSamplingMinimumSpacing = _property.FindPropertyRelative("poissonDiskSamplingMinimumSpacing");
                if (spPoissonDiskSamplingMinimumSpacing != null)
                    EditorGUILayout.PropertyField(spPoissonDiskSamplingMinimumSpacing, new GUIContent("Minimum Spacing"));

                var spPoissonDiskSamplingRadius = _property.FindPropertyRelative("poissonDiskSamplingRadius");
                if (spPoissonDiskSamplingRadius != null)
                    EditorGUILayout.PropertyField(spPoissonDiskSamplingRadius, new GUIContent("Spawn Radius"));

                var spIsPoissonDiskSamplingStaticForEverySpawn = _property.FindPropertyRelative("isPoissonDiskSamplingStaticForEverySpawn");
                if (spIsPoissonDiskSamplingStaticForEverySpawn != null)
                    EditorGUILayout.PropertyField(spIsPoissonDiskSamplingStaticForEverySpawn, new GUIContent("Keep Pattern (On Respawn)"));

                EditorGUI.indentLevel--;
            }
            else if (patternType == SpawnPattern.PatternType.RandomWithOverlap)
            {
                EditorGUI.indentLevel++;
                var spMaxRandomPositionOffset = _property.FindPropertyRelative("maxRandomPositionOffset");
                if (spMaxRandomPositionOffset != null)
                    EditorGUILayout.PropertyField(spMaxRandomPositionOffset, new GUIContent("Max Offset"));

                var spMinRandomPositionOffset = _property.FindPropertyRelative("minRandomPositionOffset");
                if (spMinRandomPositionOffset != null)
                    EditorGUILayout.PropertyField(spMinRandomPositionOffset, new GUIContent("Min Offset"));

                var spIsRandomPositionStaticForEverySpawn = _property.FindPropertyRelative("isRandomPositionStaticForEverySpawn");
                if (spIsRandomPositionStaticForEverySpawn != null)
                {
                    EditorGUILayout.PropertyField(spIsRandomPositionStaticForEverySpawn, new GUIContent("Keep Pattern (On Respawn)"));
                    GUI.enabled = spIsRandomPositionStaticForEverySpawn.boolValue;
                    var spRandomSeed = _property.FindPropertyRelative("randomSeed");
                    if (spRandomSeed != null)
                        EditorGUILayout.PropertyField(spRandomSeed);
                    GUI.enabled = true;
                }
                EditorGUI.indentLevel--;
            }
            else if (patternType == SpawnPattern.PatternType.Spiral)
            {
                EditorGUI.indentLevel++;
                var spSpiralSpacingOffset = _property.FindPropertyRelative("spiralSpacingOffset");
                if (spSpiralSpacingOffset != null)
                    EditorGUILayout.PropertyField(spSpiralSpacingOffset, new GUIContent("Spacing"));
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }
    }
}
