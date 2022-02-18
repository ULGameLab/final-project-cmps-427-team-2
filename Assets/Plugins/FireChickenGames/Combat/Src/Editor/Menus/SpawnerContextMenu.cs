namespace FireChickenGames.Combat.Editor.Menus
{
    using UnityEditor;
    using UnityEngine;

    public static class SpawnerContextMenu
    {
        static GameObject AddSpawner()
        {
            var gameObject = new GameObject
            {
                name = $"Spawner"
            };
            gameObject.AddComponent<Spawner>();
            return gameObject;
        }

        static void AddSpawner<T>() where T : Collider
        {
            var spawner = AddSpawner();
            spawner.name = $"Spawner ({typeof(T).Name})";
            var collider = spawner.AddComponent<T>();
            collider.isTrigger = true;
        }

        [MenuItem("GameObject/Fire Chicken Games/Spawner (with BoxCollider)", false, -10)]
        static void AddSpawnerWithBoxCollider()
        {
            AddSpawner<BoxCollider>();
        }

        [MenuItem("GameObject/Fire Chicken Games/Spawner (with SphereCollider)", false, -10)]
        static void AddSpawnerWithSphereCollider()
        {
            AddSpawner<SphereCollider>();
        }

        [MenuItem("GameObject/Fire Chicken Games/Spawner", false, -10)]
        static void AddSpawnerWithoutCollider()
        {
            AddSpawner();
        }
    }
}
