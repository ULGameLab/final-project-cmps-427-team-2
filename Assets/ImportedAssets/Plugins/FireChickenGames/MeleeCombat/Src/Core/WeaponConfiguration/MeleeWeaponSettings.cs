namespace FireChickenGames.MeleeCombat.Core.WeaponConfiguration
{
    using FireChickenGames.Combat.Core.WeaponConfiguration;
    using FireChickenGames.Combat.Core.WeaponConfiguration.Melee;
    using UnityEngine;

    [CreateAssetMenu(fileName = "MeleeWeaponSettings", menuName = "Fire Chicken Games/Combat/Melee Weapon Settings")]
    public class MeleeWeaponSettings : WeaponSettings, IMeleeWeaponSettings
    {
        #region Editor Fields
        public MeleeActionKey initialMeleeActionKey = MeleeActionKey.A;
        #endregion

        #region Public Property API
        public MeleeActionKey InitialMeleeActionKey { get { return initialMeleeActionKey; } set { initialMeleeActionKey = value; } }
        #endregion
    }
}
