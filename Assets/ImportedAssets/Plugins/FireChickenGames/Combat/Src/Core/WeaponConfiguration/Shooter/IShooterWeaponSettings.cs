namespace FireChickenGames.Combat.Core.WeaponConfiguration.Shooter
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface IShooterWeaponSettings
    {
        Dictionary<ScriptableObject, IAmmoSettings> RuntimeAmmoSettings { get; set; }

        bool IsAmmoAimable(ScriptableObject ammo);
        bool CanHipFire(ScriptableObject ammo);
        IAmmoSettings GetAmmoSettings(ScriptableObject ammo);
        ScriptableObject GetNextAmmo(ScriptableObject currentAmmo, bool reverse = false);
        ScriptableObject GetFirstAmmo();
        string GetFireModeDisplayName(ScriptableObject ammo, FireMode fireMode);
        FireMode GetFirstFireMode();
        void CycleToNextFireMode(ScriptableObject ammo, bool reverseSelection = false);
    }
}
