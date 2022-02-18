namespace FireChickenGames.Combat.Core.WeaponManagement
{
    using System;
    using FireChickenGames.Combat.Core.WeaponConfiguration;
    using FireChickenGames.Combat.Core.WeaponConfiguration.Melee;
    using FireChickenGames.Combat.Core.WeaponConfiguration.Shooter;
    using UnityEngine;

    [Serializable]
    public class EquippableWeapon
    {
        const int MINIMUM_BURST_AMOUNT = 1;

        public int instanceId;
        public ScriptableObject weapon;
        public ScriptableObject ammo;
        public WeaponSettings weaponSettings;
        public IMeleeWeaponSettings MeleeWeaponSettings { get { return weaponSettings as IMeleeWeaponSettings; } }
        public IShooterWeaponSettings ShooterWeaponSettings { get { return weaponSettings as IShooterWeaponSettings; } }
        protected FireMode FireMode {
            get {
                return ShooterWeaponSettings != null && ammo != null && ShooterWeaponSettings.RuntimeAmmoSettings.ContainsKey(ammo)?
                    ShooterWeaponSettings.RuntimeAmmoSettings[ammo].FireMode :
                    FireMode.None;
            }
        }
        public bool IsInAutoFireMode => FireMode == FireMode.Auto;
        public bool IsAmmoAimable => ShooterWeaponSettings?.IsAmmoAimable(ammo) ?? false;
        public bool IsHipFireTypeShoot => ShooterWeaponSettings?.CanHipFire(ammo) ?? false;

        public void Initialize()
        {
            if (ammo == null)
                ammo = GetFirstAmmo();
        }

        public ScriptableObject GetFirstAmmo()
        {
            return ShooterWeaponSettings?.GetFirstAmmo();
        }

        public bool CycleToNextAmmo(bool reverseSelection = false)
        {
            if (weaponSettings is IShooterWeaponSettings settings)
            {
                ammo = settings.GetNextAmmo(ammo, reverseSelection);
                return true;
            }
            return false;
        }

        public FireMode GetFireMode()
        {
            return FireMode;
        }

        public string GetFireModeDisplayName()
        {
            return ShooterWeaponSettings?.GetFireModeDisplayName(ammo, FireMode) ?? string.Empty;
        }

        public bool CycleToNextFireMode(bool reverseSelection = false)
        {
            var canChangeFireMode = ShooterWeaponSettings != null;
            if (canChangeFireMode)
                ShooterWeaponSettings.CycleToNextFireMode(ammo, reverseSelection);
            return canChangeFireMode;
        }

        public int GetBurstAmount()
        {
            return weaponSettings is IShooterWeaponSettings settings ?
                settings.GetAmmoSettings(ammo).GetBurstAmount() :
                MINIMUM_BURST_AMOUNT;
        }
    }
}