namespace FireChickenGames.Combat.Core
{
    using GameCreator.Core.Hooks;
    using UnityEngine;

    public class HookManager
    {
        public static Camera GetCamera()
        {
            var camera = HookCamera.Instance?.Get<Camera>();
            return !camera ? GameObject.FindObjectOfType<Camera>() : camera;
        }

        public static GameObject GetPlayer()
        {
            var instanceGameObject = HookPlayer.Instance?.gameObject;
            return instanceGameObject == null ? GameObject.FindWithTag("Player") : instanceGameObject;
        }

        public static WeaponStashUi GetWeaponStashUi()
        {
            return HookWeaponStashUi.Instance == null ? null : HookWeaponStashUi.Instance.GetComponent<WeaponStashUi>();
        }

        public static Targeter GetPlayerTargeter()
        {
            var player = GetPlayer();
            return player ?? false ? player.GetComponentInChildren<Targeter>() : null;
        }
    }
}
