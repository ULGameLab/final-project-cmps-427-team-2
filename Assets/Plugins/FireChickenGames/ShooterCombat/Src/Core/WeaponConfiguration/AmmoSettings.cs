namespace FireChickenGames.ShooterCombat.Core.WeaponConfiguration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FireChickenGames.Combat.Core.WeaponConfiguration.Shooter;
    using GameCreator.Shooter;
    using UnityEngine;

    [Serializable]
    public class AmmoSettings : IAmmoSettings
    {
        #region Editor Fields
        public Ammo ammo;

        public bool isAmmoAimable = true;
        public HipFireType hipFireType = HipFireType.Shoot;

        public bool hasSingleShotMode;

        public bool hasBurstMode;
        [Range(0, 30)]
        public int burstAmount = 5;

        public bool hasFullAutoMode;

        public string fireModeNameNone = "";
        public string fireModeNameSingle = "";
        public string fireModeNameBurst = "Burst";
        public string fireModeNameAuto = "Auto";
        #endregion 

        #region Public Property API
        public ScriptableObject Ammo { get { return ammo; } }
        public bool IsHipFireTypeShoot => hipFireType == HipFireType.Shoot;
        public FireMode FireMode { get; set; }
        public bool IsAmmoAimable => isAmmoAimable;
        #endregion

        #region Public API
        public LinkedList<FireMode> GetFireModes(bool reverse = false)
        {
            var fireModes = new List<FireMode>();
            
            if (hasSingleShotMode)
                fireModes.Add(FireMode.Single);
            if (hasBurstMode)
                fireModes.Add(FireMode.Burst);
            if (hasFullAutoMode)
                fireModes.Add(FireMode.Auto);

            if (reverse)
                fireModes.Reverse();

            return new LinkedList<FireMode>(fireModes);
        }

        public int GetBurstAmount()
        {
            return burstAmount;
        }

        public FireMode GetFirstFireMode()
        {
            return GetFireModes().FirstOrDefault();
        }

        public FireMode GetNextFireMode(FireMode fireMode, bool reverse = false)
        {
            var availableFireModes = GetFireModes(reverse);

            if (!availableFireModes.Any())
                return fireMode;

            if (availableFireModes.Count == 1)
                return availableFireModes.First();

            var listAtCurrentFireMode = availableFireModes.Find(fireMode);
            return listAtCurrentFireMode == null ?
                availableFireModes.First.Value :
                (listAtCurrentFireMode.Next ?? availableFireModes.First).Value;
        }

        public string GetFireModeDisplayName(FireMode fireMode)
        {
            if (fireMode == FireMode.Single)
                return fireModeNameSingle;
            else if (fireMode == FireMode.Burst)
                return fireModeNameBurst;
            else if (fireMode == FireMode.Auto)
                return fireModeNameAuto;
            return fireModeNameNone;
        }

        public static IAmmoSettings Make(AmmoSettings ammoSettings)
        {
            return new AmmoSettings
            {
                isAmmoAimable = ammoSettings.isAmmoAimable,
                hasSingleShotMode = ammoSettings.hasSingleShotMode,
                hasBurstMode = ammoSettings.hasBurstMode,
                burstAmount = ammoSettings.burstAmount,
                hasFullAutoMode = ammoSettings.hasFullAutoMode,
                hipFireType = ammoSettings.hipFireType,
                fireModeNameNone = ammoSettings.fireModeNameNone,
                fireModeNameSingle = ammoSettings.fireModeNameSingle,
                fireModeNameBurst = ammoSettings.fireModeNameBurst,
                fireModeNameAuto = ammoSettings.fireModeNameAuto,

                FireMode = ammoSettings.GetFirstFireMode(),
            };
        }
        #endregion
    }
}
