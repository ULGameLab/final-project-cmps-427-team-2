namespace FireChickenGames.Combat.Core.Spawning
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class SpawnPattern
    {
        public enum PatternType
        {
            Default,
            Spiral,
            RandomWithOverlap,
            AtSpawnerOrigin
        }

        public PatternType patternType = PatternType.Default;

        // Poisson Disk Sampling
        [Tooltip("The minimum distance between spawned objects.")]
        public float poissonDiskSamplingMinimumSpacing = 1.0f;
        [Tooltip("The radius, from the center of the Spawner, to spawn objects with-in.")]
        public float poissonDiskSamplingRadius = 5.0f;
        [Tooltip("If enabled, the spawn position will not change when a respawn happens.")]
        public bool isPoissonDiskSamplingStaticForEverySpawn;
        public List<Vector3> spawnPoints = new List<Vector3>();

        // Spiral
        [Tooltip("The distance between spawned objects.")]
        public float spiralSpacingOffset = 1.0f;

        // Random
        [Tooltip("If enabled, the spawn position will not change when a respawn happens.")]
        public bool isRandomPositionStaticForEverySpawn;
        [Tooltip("Change this variable to randomize the spawned object's positions.")]
        public int randomSeed = 1;
        [Tooltip("The maximum bounds for the area where the objects are spawned.")]
        public Vector3 maxRandomPositionOffset = new Vector3(10f, 0.0f, 10.0f);
        [Tooltip("The minimum bounds for the area where the objects are spawned.")]
        public Vector3 minRandomPositionOffset = new Vector3(-10f, 0.0f, -10.0f);

        // Boolean properties to simplify pattern type check.
        public bool IsPoissonDiskSampling { get { return patternType == PatternType.Default; } }
        public bool IsSpiral { get { return patternType == PatternType.Spiral; } }
        public bool IsRandom { get { return patternType == PatternType.RandomWithOverlap; } }
        public bool IsAtSpawnerOrigin { get { return patternType == PatternType.AtSpawnerOrigin; } }

        public override string ToString()
        {
            if (IsPoissonDiskSampling)
                return "Poisson-disk Sampling";
            else if (IsRandom)
                return "Random";
            else if (IsSpiral)
                return "Spiral";
            else if (IsAtSpawnerOrigin)
                return "At Spawner Origin";
            return "Unknown";
        }
    }
}
