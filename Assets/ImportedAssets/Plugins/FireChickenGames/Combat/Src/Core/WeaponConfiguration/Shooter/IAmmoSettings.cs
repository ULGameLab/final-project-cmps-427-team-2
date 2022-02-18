namespace FireChickenGames.Combat.Core.WeaponConfiguration.Shooter
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface IAmmoSettings
    {
        ScriptableObject Ammo { get; }
        FireMode FireMode { get; set; }
        bool IsHipFireTypeShoot { get; }
        bool IsAmmoAimable { get; }

        LinkedList<FireMode> GetFireModes(bool reverse = false);
        FireMode GetFirstFireMode();
        FireMode GetNextFireMode(FireMode fireMode, bool reverse = false);
        string GetFireModeDisplayName(FireMode fireMode);
        int GetBurstAmount();
    }
}
