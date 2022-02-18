namespace FireChickenGames.Combat.Core.Spawning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    [Serializable]
    public class Spawnable
    {
        public Spawnable(GameObject gameObjectToSpawn, int quantityToSpawn = 0, int weightOfSpawnable = 100)
        {
            gameObject = gameObjectToSpawn;
            quantity = quantityToSpawn;
            weight = weightOfSpawnable;
        }

        [SerializeField]
        public GameObject gameObject;
        [SerializeField]
        public int quantity;
        [SerializeField]
        [Range(0, 100)]
        public int weight;

        public List<GameObject> InstancePool { get; set; }

        public Spawnable()
        {
            InstancePool = new List<GameObject>();
        }

        public void ShuffleInstances()
        {
            InstancePool = InstancePool.OrderBy(x => UnityEngine.Random.value).ToList();
        }
    }
}
