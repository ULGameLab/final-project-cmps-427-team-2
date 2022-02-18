namespace FireChickenGames.Combat.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(Spawner))]
    public class SpawnerEditor : Editor
    {
        protected Spawner instance;

        public SerializedProperty targetGameObject;

        // Spawning
        private SerializedProperty isSnapToGroundEnabled;
        private SerializedProperty spawnPattern;
        private SerializedProperty spawnables;
        private SerializedProperty isSpawnPeriodicallyEnabled;
        private SerializedProperty periodicSpawnInSeconds;
        private SerializedProperty periodicSkipChance;
        private SerializedProperty periodicSpawnAfterSkipInSeconds;
        private SerializedProperty isSpawnByWeightEnabled;

        // Respawn
        private SerializedProperty isRespawningEnabled;
        private SerializedProperty isRespawnCooldownEnabled;
        private SerializedProperty respawnCooldownInSeconds;

        // Despawning
        private SerializedProperty isDespawnByDistanceEnabled;
        private SerializedProperty isDespawnDistanceAutoCalculated;
        private SerializedProperty despawnDistanceFromClosestSpawnable;

        protected void OnEnable()
        {
            instance = target as Spawner;

            targetGameObject = serializedObject.FindProperty("targetGameObject");

            // Spawning
            isSnapToGroundEnabled = serializedObject.FindProperty("isSnapToGroundEnabled");
            spawnPattern = serializedObject.FindProperty("spawnPattern");
            spawnables = serializedObject.FindProperty("spawnables");
            isSpawnPeriodicallyEnabled = serializedObject.FindProperty("isSpawnPeriodicallyEnabled");
            periodicSpawnInSeconds = serializedObject.FindProperty("periodicSpawnInSeconds");
            periodicSkipChance = serializedObject.FindProperty("periodicSkipChance");
            periodicSpawnAfterSkipInSeconds = serializedObject.FindProperty("periodicSpawnAfterSkipInSeconds");
            isSpawnByWeightEnabled = serializedObject.FindProperty("isSpawnByWeightEnabled");

            // Respawning
            isRespawningEnabled = serializedObject.FindProperty("isRespawningEnabled");
            isRespawnCooldownEnabled = serializedObject.FindProperty("isRespawnCooldownEnabled");
            respawnCooldownInSeconds = serializedObject.FindProperty("respawnCooldownInSeconds");

            // Despawning
            isDespawnByDistanceEnabled = serializedObject.FindProperty("isDespawnByDistanceEnabled");
            isDespawnDistanceAutoCalculated = serializedObject.FindProperty("isDespawnDistanceAutoCalculated");
            despawnDistanceFromClosestSpawnable = serializedObject.FindProperty("despawnDistanceFromClosestSpawnable");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var headerLabelHeight = GUILayout.Height(20.0f);

            EditorGUILayout.PropertyField(targetGameObject, new GUIContent("Target"));
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            // Spawning
            EditorGUILayout.LabelField("Spawning", EditorStyles.boldLabel, headerLabelHeight);
            EditorGUILayout.PropertyField(isSnapToGroundEnabled, new GUIContent("Snap to Ground"));
            EditorGUILayout.PropertyField(spawnPattern, new GUIContent("Pattern"));
            EditorGUILayout.PropertyField(isSpawnPeriodicallyEnabled, new GUIContent("Periodically Spawn"));
            if (instance.isSpawnPeriodicallyEnabled)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(periodicSpawnInSeconds, new GUIContent("Spawn Rate (Seconds)"));
                EditorGUILayout.PropertyField(periodicSkipChance, new GUIContent("Skip Chance"));
                EditorGUILayout.PropertyField(periodicSpawnAfterSkipInSeconds, new GUIContent("Spawn Rate After Skip (Seconds)"));
                EditorGUI.indentLevel--;
            }

            // Spawn by Weight
            var spawnableItemPrefix = "Item";
            var spawnablesPercentageText = "";

            if (isSpawnByWeightEnabled.boolValue)
            {
                var spawnableWeights = new List<int>();
                for (int i = 0; i < spawnables.arraySize; i++)
                {
                    var spawnableItem = spawnables.GetArrayElementAtIndex(i);
                    var weight = spawnableItem.FindPropertyRelative("weight").intValue;
                    spawnableWeights.Add(weight);
                }

                List<float> spawnablePercentages = new List<float>();
                var sumOfWeight = (float)spawnableWeights.Sum();
                foreach (var spawnableWeight in spawnableWeights)
                    spawnablePercentages.Add(spawnableWeight == 0 ? 0.0f : Mathf.Floor((spawnableWeight / sumOfWeight) * 100));

                for (int i = 0; i < spawnables.arraySize; i++)
                {
                    var percentageText = $"{spawnablePercentages[i]}%".PadLeft(5, ' ');
                    spawnablesPercentageText += $"{percentageText} {spawnableItemPrefix} {i + 1}{Environment.NewLine}";
                }
            }

            EditorGUILayout.PropertyField(isSpawnByWeightEnabled, new GUIContent("Spawn by Weight"));
            if (isSpawnByWeightEnabled.boolValue)
                EditorGUILayout.HelpBox(spawnablesPercentageText, MessageType.None, false);

            // Spawnables
            var arraySizeProp = spawnables.FindPropertyRelative("Array.size");
            EditorGUILayout.PropertyField(arraySizeProp, new GUIContent("Spawnables"));
            EditorGUI.indentLevel++;
            for (int i = 0; i < spawnables.arraySize; i++)
            {
                var spawnableItem = spawnables.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(spawnableItem, new GUIContent($"{spawnableItemPrefix} {i + 1}"));
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            // Respawning
            EditorGUILayout.LabelField("Respawning", EditorStyles.boldLabel, headerLabelHeight);
            EditorGUILayout.PropertyField(isRespawningEnabled, new GUIContent("Enabled"));
            EditorGUILayout.PropertyField(isRespawnCooldownEnabled, new GUIContent("Cooldown"));
            GUI.enabled = isRespawnCooldownEnabled.boolValue;
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(respawnCooldownInSeconds, new GUIContent("Duration (Seconds)"));
            EditorGUI.indentLevel--;
            GUI.enabled = true;
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            // Despawning
            EditorGUILayout.LabelField("Despawning", EditorStyles.boldLabel, headerLabelHeight);
            EditorGUILayout.PropertyField(isDespawnByDistanceEnabled, new GUIContent("Despawn by Distance"));
            EditorGUI.indentLevel++;
            GUI.enabled = isDespawnByDistanceEnabled.boolValue;
            EditorGUILayout.PropertyField(isDespawnDistanceAutoCalculated, new GUIContent("Auto-calculate Distance"));
            EditorGUILayout.HelpBox(new GUIContent("The auto-calculated spawn distance is the radius of an attached SphereCollider or the average height/width of an attached BoxCollider."), false);
            GUI.enabled = isDespawnByDistanceEnabled.boolValue && !isDespawnDistanceAutoCalculated.boolValue;
            EditorGUILayout.PropertyField(despawnDistanceFromClosestSpawnable, new GUIContent("Despawn Distance"), false);
            GUI.enabled = true;
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
