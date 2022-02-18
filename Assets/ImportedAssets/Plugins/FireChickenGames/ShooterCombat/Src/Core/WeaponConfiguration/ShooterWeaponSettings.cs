namespace FireChickenGames.ShooterCombat.Core.WeaponConfiguration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FireChickenGames.Combat.Core.WeaponConfiguration;
    using FireChickenGames.Combat.Core.WeaponConfiguration.Shooter;
    using GameCreator.Shooter;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ShooterWeaponSettings", menuName = "Fire Chicken Games/Combat/Shooter Weapon Settings")]
    public class ShooterWeaponSettings : WeaponSettings, IShooterWeaponSettings
    {
        public List<AmmoSettings> ammoSettings = new List<AmmoSettings>();

        [Header("Fire Modes")]
        public bool shouldOverrideAmmoSettingsFireModeNames = false;
        public string fireModeNameNone = "";
        public string fireModeNameSingle = "";
        public string fireModeNameBurst = "Burst";
        public string fireModeNameAuto = "Auto";

        #region Public Field API
        public Dictionary<ScriptableObject, IAmmoSettings> RuntimeAmmoSettings { get; set; } = new Dictionary<ScriptableObject, IAmmoSettings>();
        #endregion

        public void OnEnable()
        {
            RuntimeAmmoSettings = MakeRuntimeAmmoSettings();
        }

        public bool IsAmmoAimable(ScriptableObject ammo)
        {
            return ammo != null && RuntimeAmmoSettings.ContainsKey(ammo) && RuntimeAmmoSettings[ammo].IsAmmoAimable;
        }

        public bool CanHipFire(ScriptableObject ammo)
        {
           return ammo != null && RuntimeAmmoSettings.ContainsKey(ammo) && RuntimeAmmoSettings[ammo].IsHipFireTypeShoot;
        }

        public FireMode GetFirstFireMode()
        {
            var settings = GetSettingsConfiguredWithAmmo().FirstOrDefault();
            return settings == null ? FireMode.None : settings.GetFirstFireMode();
        }

        public string GetFireModeDisplayName(ScriptableObject ammo, FireMode fireMode)
        {
            if (shouldOverrideAmmoSettingsFireModeNames)
            { 
                if (fireMode == FireMode.Single)
                    return fireModeNameSingle;
                else if (fireMode == FireMode.Burst)
                    return fireModeNameBurst;
                else if (fireMode == FireMode.Auto)
                    return fireModeNameAuto;
            }
            else if (ammo != null && RuntimeAmmoSettings.ContainsKey(ammo))
                return RuntimeAmmoSettings[ammo].GetFireModeDisplayName(fireMode);
            return fireModeNameNone;
        }

        public IAmmoSettings GetAmmoSettings(ScriptableObject ammo)
        {
            return ammoSettings.FirstOrDefault(x => x.ammo == ammo);
        }

        protected List<AmmoSettings> GetSettingsConfiguredWithAmmo()
        {
            return ammoSettings.Where(x => x.ammo != null && x.ammo is Ammo).ToList();
        }

        public ScriptableObject GetFirstAmmo()
        {
            return GetSettingsConfiguredWithAmmo().FirstOrDefault()?.ammo;
        }

        public ScriptableObject GetNextAmmo(ScriptableObject currentAmmo, bool reverse = false)
        {
            var currentAmmoSettings = GetAmmoSettings(currentAmmo);
            var settingsConfiguredWithAmmo = GetSettingsConfiguredWithAmmo();
            if (!settingsConfiguredWithAmmo.Any())
                return currentAmmoSettings.Ammo;

            if (settingsConfiguredWithAmmo.Count == 1)
                return settingsConfiguredWithAmmo.First().ammo;

            if (reverse)
                settingsConfiguredWithAmmo.Reverse();

            var ammoSettingsAsLinkedList = new LinkedList<IAmmoSettings>(settingsConfiguredWithAmmo);
            var linkedListNode = ammoSettingsAsLinkedList.Find(currentAmmoSettings);
            return linkedListNode == null ?
                ammoSettingsAsLinkedList.First.Value.Ammo :
                (linkedListNode.Next ?? ammoSettingsAsLinkedList.First).Value.Ammo;
        }

        public Dictionary<ScriptableObject, IAmmoSettings> MakeRuntimeAmmoSettings()
        {
            var map = new Dictionary<ScriptableObject, IAmmoSettings>();
            var configuredAmmoSettings = GetSettingsConfiguredWithAmmo();
            foreach (var settings in configuredAmmoSettings)
            {
                if (!map.ContainsKey(settings.ammo))
                    map.Add(settings.ammo, AmmoSettings.Make(settings));
            }
            return map;
        }

        public void CycleToNextFireMode(ScriptableObject ammo, bool reverseSelection = false)
        {
            if (RuntimeAmmoSettings.ContainsKey(ammo))
            {
                RuntimeAmmoSettings[ammo].FireMode = RuntimeAmmoSettings[ammo].GetNextFireMode(RuntimeAmmoSettings[ammo].FireMode, reverseSelection);
            }
        }
    }
}
