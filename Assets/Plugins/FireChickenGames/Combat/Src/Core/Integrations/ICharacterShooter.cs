namespace FireChickenGames.Combat.Core.Integrations
{
    using System.Collections;
    using FireChickenGames.Combat.Core.WeaponManagement;
    using UnityEngine;
    using UnityEngine.Events;

    public interface ICharacterShooter
    {
        bool IsFreeToAct { get; }
        bool IsControllable { get; }
        bool IsCharacterLocomotionBusy { get; }

        bool IsDrawing { get; }
        bool IsHolstering { get; }
        bool IsReloading { get; }

        bool IsAiming { get; }
        bool IsChargingShot { get; }
        ScriptableObject CurrentWeapon { get; }
        ScriptableObject CurrentAmmo { get; }

        WeaponStashUi WeaponStashUi { get; set; }
        UnityAction<ScriptableObject> SetStashedWeapon { get; set; }

        bool IsAutoShooting { get; }

        bool HasCharacterShooter();
        void SetCharacterShooter(Component characterShooter);
        Component GetCharacterShooter();

        IEnumerator ChangeWeapon(ScriptableObject weapon = null, ScriptableObject ammo = null);
        void SetWeaponNameAndDescription(ScriptableObject stashedWeapon);

        void ChangeAmmo(ScriptableObject ammo);
        void SetAmmoNameAndDescription(ScriptableObject ammo);
        bool IsShooterWeapon(ScriptableObject weapon);
        void SetAmmoNameAndDescriptionFromWeapon(ScriptableObject weapon);
        bool IsClipEmpty();
        IEnumerator Reload();

        void DestroyCrosshair();

        // Shooting
        IEnumerator Shoot(EquippableWeapon equippableWeapon);
        IEnumerator StopAutoShooting();
        void StopAutoShootingInstant();
        bool IsChargeShotRequired(ScriptableObject ammo);
        void StartChargedShot();
        void ExecuteChargedShot();

        // Aiming
        void AddEventOnAimListener(UnityAction<bool> onAim);
        void RemoveEventOnAimListener(UnityAction<bool> onAim);
        void StartAiming(IAimingAtProximityTarget aimingAtTarget = null, bool isCrosshairVisible = true);
        void StopAiming();
        GameObject GetAimAssistTarget();

        // Ammo
        void AddEventChangeAmmoListener();
        void RemoveEventChangeAmmoListener();

        // Clip
        void AddEventChangeClipListener(UnityAction<string, int> setAmmoInClip);
        void RemoveEventChangeClipListener(UnityAction<string, int> setAmmoInClip);

        // Storage
        void AddEventChangeStorageListener(UnityAction<string, int> setAmmoInStorage);
        void RemoveEventChangeStorageListener(UnityAction<string, int> setAmmoInStorage);
    }
}
