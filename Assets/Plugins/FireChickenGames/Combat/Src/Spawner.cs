namespace FireChickenGames.Combat
{
    using System.Collections.Generic;
    using System.Linq;
    using FireChickenGames.Combat.Core.Spawning;
    using GameCreator.Core;
    using UnityEngine;
    using UnityRandom = UnityEngine.Random;

    [AddComponentMenu("Fire Chicken Games/Combat/Spawner")]
    public class Spawner : MonoBehaviour
    {
        [Tooltip("The GameObject that triggers the spawn (if a collider is attached to the Spawner).")]
        public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.Player);

        // Spawn.
        [Tooltip("When enabled, sets the Y position of the spawned object level with the ground. This prevents objects from being spawned under terrain.")]
        public bool isSnapToGroundEnabled = true;
        public SpawnPattern spawnPattern = new SpawnPattern();
        public bool isSpawnPeriodicallyEnabled = false;
        public float periodicSpawnInSeconds = 1.0f;
        [Range(0,100)]
        public int periodicSkipChance;
        public float periodicSpawnAfterSkipInSeconds = 1.0f;

        // Respawn.
        [Tooltip("If enabled, respawning will be possible.")]
        public bool isRespawningEnabled = true;
        [Tooltip("If enabled, a respawn will only happen after a specified number of seconds.")]
        public bool isRespawnCooldownEnabled = true;
        [Tooltip("The number of seconds to wait to respawn.")]
        public int respawnCooldownInSeconds = 10;

        // Despawn.
        [Tooltip("If enabled, spawned objects will be automatically despawned when the object that triggered the spawn leaves the spawn zone and is a set distance from the closest spawned object.")]
        public bool isDespawnByDistanceEnabled = true;
        [Tooltip("If enabled, the despawning distance will be automatically computed.")]
        public bool isDespawnDistanceAutoCalculated = true;
        [Tooltip("The minimum distance between the object that triggered the spawn and the closest spawned object before despawning is triggered.")]
        public float despawnDistanceFromClosestSpawnable = 0.0f;

        // Objects to spawn.
        [Tooltip("A collection of GameObjects to spawn.")]
        public List<Spawnable> spawnables = new List<Spawnable>();
        [Tooltip("If enabled, each spawnable is spawned randomly, some percentage of the time based on its weight property.")]
        public bool isSpawnByWeightEnabled;

        public bool isSpawnActivatable;
        public bool hasDespawned;
        public float respawnCooldownCounter;
        private float periodicSpawnCounter;
        private int periodicSpawnInstanceIndex;

        void Start()
        {
            TryGetComponent<Collider>(out var colliderComponent);
            if (isDespawnDistanceAutoCalculated && colliderComponent != null)
            {
                if (colliderComponent is SphereCollider)
                    despawnDistanceFromClosestSpawnable = (colliderComponent as SphereCollider).radius;
                else if (colliderComponent is BoxCollider)
                {
                    var boxCollider = colliderComponent as BoxCollider;
                    // Despawn distance is the average width and height.
                    despawnDistanceFromClosestSpawnable = (boxCollider.size.x + boxCollider.size.z) / 2;
                }
            }

            InstantiateSpawnableObjectPools(spawnables);
        }

        void Update()
        {
            var instancePool = spawnables.SelectMany(x => x.InstancePool).ToList();
            var isAtLeastOneInstanceActive = instancePool.Where(x => x != null && x.activeSelf).Any();  
            if (isSpawnActivatable && !isAtLeastOneInstanceActive)
                PostDespawn();
            if (isDespawnByDistanceEnabled && !isSpawnActivatable && isAtLeastOneInstanceActive)
            {
                var distanceToClosestSpawnable = instancePool
                    .Where(x => x != null)
                    .Select(x => Vector3.Distance(GetTargetGameObject().transform.position, x.transform.position))
                    .Min();
                if (distanceToClosestSpawnable > despawnDistanceFromClosestSpawnable)
                    Despawn();
            }

            if (hasDespawned && respawnCooldownCounter > 0.0f)
                respawnCooldownCounter -= Time.deltaTime;
        }

        void OnTriggerExit(Collider targetCollider)
        {
            if (targetCollider.gameObject == GetTargetGameObject())
            {
                isSpawnActivatable = false;
            }
        }

        void OnTriggerEnter(Collider targetCollider)
        {
            if (targetCollider.gameObject == GetTargetGameObject())
            {
                isSpawnActivatable = true;
                Spawn();
            }
        }

        void OnTriggerStay(Collider targetCollider)
        {
            if (targetCollider.gameObject == GetTargetGameObject())
            {
                if (isSpawnPeriodicallyEnabled)
                {
                    if (periodicSpawnCounter <= 0.0f)
                    {
                        periodicSpawnCounter = periodicSpawnInSeconds;
                        if (new System.Random().Next(100) < periodicSkipChance)
                            periodicSpawnCounter = periodicSpawnAfterSkipInSeconds;
                        else
                            Spawn();
                    }
                    else
                        periodicSpawnCounter -= Time.deltaTime;
                }
                else if (!isSpawnActivatable)
                { 
                    isSpawnActivatable = true;
                    Spawn();
                }
            }
        }

        void InstantiateSpawnable(Spawnable spawnable, Vector3 instancePosition, bool isActive = true)
        {
            if (spawnable?.gameObject == null)
                return;
            var instance = Instantiate(spawnable.gameObject, instancePosition, transform.rotation, transform);
            instance.SetActive(isActive);
            spawnable.InstancePool.Add(instance);
        }

        void InstantiateSpawnableObjectPools(List<Spawnable> spawnables)
        {
            if (spawnables == null || !spawnables.Any() || spawnables.SelectMany(x => x.InstancePool).Any())
                return;

            foreach (var spawnable in spawnables)
            {
                for (int i = 0; i < spawnable.quantity; i++)
                    InstantiateSpawnable(spawnable, transform.position, false);
                spawnable.ShuffleInstances();
            }
        }

        List<GameObject> SelectInstancePool(bool getAll = false)
        {
            if (getAll || !isSpawnByWeightEnabled)
                return spawnables.SelectMany(x => x.InstancePool).ToList();

            var sumOfWeight = spawnables.Sum(x => x.weight);
            var rand = new System.Random().Next(sumOfWeight);
            return spawnables.FirstOrDefault(x => rand >= (sumOfWeight -= x.weight))?.InstancePool;
        }

        void SpawnWithPoissonDiskSamplingPattern(List<GameObject> instancePool)
        {
            var origin = new Vector2(transform.position.x, transform.position.z);
            if (spawnPattern.spawnPoints == null ||
                !spawnPattern.spawnPoints.Any() ||
                !spawnPattern.isPoissonDiskSamplingStaticForEverySpawn)
                spawnPattern.spawnPoints = PoissonDiskSampling
                    .SampleCircle(origin, spawnPattern.poissonDiskSamplingRadius, spawnPattern.poissonDiskSamplingMinimumSpacing)
                    .OrderBy(x => UnityRandom.value)
                    .Select(point => new Vector3(point.x, transform.position.y, point.y))
                    .ToList();

            SetInstancesToActiveAndPosition(instancePool, true);
        }

        void SpawnWithSpiralPattern(List<GameObject> instancePool)
        {
            spawnPattern.spawnPoints = new List<Vector3>();

            var coils = 1;
            var radius = 1.0f;
            var thetaMax = coils * 2 * Mathf.PI;

            // Distance to translate from the parent transform.
            var awayStep = radius / thetaMax;
            var rotation = 2 * Mathf.PI;
            var theta = spawnPattern.spiralSpacingOffset / awayStep;

            for (int i = 0; i < instancePool.Count; i++)
            {
                var away = awayStep * theta;
                var around = theta + rotation;

                var x = transform.position.x + Mathf.Cos(around) * away;
                var z = transform.position.z + Mathf.Sin(around) * away;

                spawnPattern.spawnPoints.Add(new Vector3(x, transform.position.y, z));

                theta += spawnPattern.spiralSpacingOffset / away;
            }

            SetInstancesToActiveAndPosition(instancePool);
        }

        void SpawnWithRandomPattern(List<GameObject> instancePool)
        {
            spawnPattern.spawnPoints = new List<Vector3>();

            if (spawnPattern.isRandomPositionStaticForEverySpawn)
                UnityRandom.InitState(spawnPattern.randomSeed);

            for (int i = 0; i < instancePool.Count; i++)
            { 
                var spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                spawnPosition.x += UnityRandom.Range(spawnPattern.minRandomPositionOffset.x, spawnPattern.maxRandomPositionOffset.x);
                spawnPosition.y += UnityRandom.Range(spawnPattern.minRandomPositionOffset.y, spawnPattern.maxRandomPositionOffset.y);
                spawnPosition.z += UnityRandom.Range(spawnPattern.minRandomPositionOffset.z, spawnPattern.maxRandomPositionOffset.z);
                spawnPattern.spawnPoints.Add(spawnPosition);
            }

            SetInstancesToActiveAndPosition(instancePool);
        }

        void SpawnWithAtSpawnerOriginPattern(List<GameObject> instancePool)
        {
            spawnPattern.spawnPoints.Clear();
            SetInstancesToActiveAndPosition(instancePool);
        }

        void SetInstancesToActiveAndPosition(List<GameObject> instancePool, bool limitToSpawnPoints = false)
        {
            var defaultPosition = spawnPattern.spawnPoints.Any() && periodicSpawnInstanceIndex < spawnPattern.spawnPoints.Count ?
                spawnPattern.spawnPoints[periodicSpawnInstanceIndex] :
                new Vector3(transform.position.x, transform.position.y, transform.position.z);

            if (isSpawnPeriodicallyEnabled)
            {
                if (limitToSpawnPoints && periodicSpawnInstanceIndex >= spawnPattern.spawnPoints.Count)
                    return;
                if (periodicSpawnInstanceIndex >= instancePool.Count)
                    return;
                SetInstanceToActiveAndPosition(instancePool[periodicSpawnInstanceIndex], defaultPosition);
                periodicSpawnInstanceIndex++;
            }
            else
            {
                for (var i = 0; i < instancePool.Count; i++)
                {
                    if (limitToSpawnPoints && i >= spawnPattern.spawnPoints.Count)
                        break;

                    SetInstanceToActiveAndPosition(
                        instancePool[i],
                        spawnPattern.spawnPoints.Any() ? spawnPattern.spawnPoints[i] : defaultPosition
                    ); ;
                }
            }
        }

        void SetInstanceToActiveAndPosition(GameObject instance, Vector3 position)
        {
            if (isSnapToGroundEnabled)
            {
                var positionToRaycastFrom = new Vector3(position.x, position.y + 0.25f, position.z);
                if (Physics.Raycast(positionToRaycastFrom, -Vector3.up, out RaycastHit hit))
                    position.y = hit.point.y;
                else
                    // Snap to ground is enabled, but there is no ground under the spawn point to place the object.
                    return;
            }

            instance.transform.position = position;
            instance.SetActive(true);
        }

        void PostDespawn()
        {
            if (hasDespawned)
                return;

            hasDespawned = true;
            respawnCooldownCounter = respawnCooldownInSeconds;
            periodicSpawnCounter = periodicSpawnInSeconds;
            periodicSpawnInstanceIndex = 0;
        }

        GameObject GetTargetGameObject()
        {
            return targetGameObject.GetGameObject(gameObject);
        }

        /**
         * Public API
         */
        public void Spawn()
        {
            var instancePool = SelectInstancePool();
            if (spawnables == null || !spawnables.Any() || (!isSpawnPeriodicallyEnabled && instancePool.Where(x => x.activeSelf).Any()))
                return;

            if (!isRespawningEnabled && hasDespawned)
                return;

            if (hasDespawned && isRespawnCooldownEnabled && respawnCooldownCounter > 0.0f)
                return;

            if (isSpawnPeriodicallyEnabled && periodicSpawnInstanceIndex >= spawnPattern.spawnPoints.Count && spawnPattern.spawnPoints.Any())
                // Do not bother continuing when all the instances are spawned while using periodic spawning.
                return;

            hasDespawned = false;

            if (spawnPattern.IsPoissonDiskSampling)
                SpawnWithPoissonDiskSamplingPattern(instancePool);
            else if (spawnPattern.IsRandom)
                SpawnWithRandomPattern(instancePool);
            else if (spawnPattern.IsSpiral)
                SpawnWithSpiralPattern(instancePool);
            else if (spawnPattern.IsAtSpawnerOrigin)
                SpawnWithAtSpawnerOriginPattern(instancePool);
        }

        public void Despawn()
        {
            var instancePool = SelectInstancePool(true);
            if (instancePool == null || !instancePool.Any())
                return;

            foreach (var instance in instancePool)
                instance.SetActive(false);

            PostDespawn();
        }
    }
}
