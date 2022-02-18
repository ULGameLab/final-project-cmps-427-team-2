namespace FireChickenGames.MeleeCombat.Core.Integrations
{
    using FireChickenGames.Combat.Core.Integrations;
    using FireChickenGames.Combat.Core.WeaponConfiguration.Melee;
    using FireChickenGames.Combat.Core.WeaponManagement;
    using GameCreator.Melee;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using static GameCreator.Melee.CharacterMelee;

    public class CharacterMeleeAdapter : ICharacterMelee
    {
        public CharacterMelee characterMelee;

        public ScriptableObject CurrentWeapon => characterMelee?.currentWeapon;
        public ScriptableObject CurrentShield => characterMelee?.currentShield;

        public bool IsBlocking => characterMelee != null && characterMelee.IsBlocking;

        public static Dictionary<MeleeActionKey, ActionKey> actionKeyMap = new Dictionary<MeleeActionKey, ActionKey>() {
            { MeleeActionKey.A, ActionKey.A},
            { MeleeActionKey.B, ActionKey.B},
            { MeleeActionKey.C, ActionKey.C},
            { MeleeActionKey.D, ActionKey.D},
            { MeleeActionKey.E, ActionKey.E},
            { MeleeActionKey.F, ActionKey.F},
        };

        public void SetCharacterMelee(Component characterMelee)
        {
            this.characterMelee = characterMelee as CharacterMelee;
        }

        public IEnumerator Draw(ScriptableObject meleeWeapon, ScriptableObject shield = null)
        {
            if (characterMelee != null && meleeWeapon is MeleeWeapon && (shield is MeleeShield || shield == null))
                yield return characterMelee.Draw(meleeWeapon as MeleeWeapon, shield as MeleeShield);
        }

        public void TakeWeapon()
        {
            if (characterMelee != null)
                characterMelee.currentWeapon = null;
        }

        public IEnumerator Sheathe()
        {
            if (characterMelee != null)
                yield return characterMelee.Sheathe();
        }

        public string GetWeaponName(ScriptableObject meleeWeapon)
        {
            return meleeWeapon is MeleeWeapon ? (meleeWeapon as MeleeWeapon).weaponName.GetText() : "";
        }

        public string GetWeaponDescription(ScriptableObject meleeWeapon)
        {
            return meleeWeapon is MeleeWeapon ? (meleeWeapon as MeleeWeapon).weaponDescription.GetText() : "";
        }

        public bool IsMeleeWeapon(ScriptableObject weapon)
        {
            return weapon is MeleeWeapon;
        }

        public void Attack(EquippableWeapon equippableWeapon)
        {
            var meleeActionKey = equippableWeapon != null || equippableWeapon.MeleeWeaponSettings != null?
                equippableWeapon.MeleeWeaponSettings.InitialMeleeActionKey :
                MeleeActionKey.A;
            characterMelee.Execute(actionKeyMap[meleeActionKey]);
        }

        public void StartBlocking()
        {
            characterMelee.StartBlocking();
        }

        public void StopBlocking()
        {
            characterMelee.StopBlocking();
        }

        public void SetInvincibility(float duration)
        {
            characterMelee.SetInvincibility(duration);
        }
    }
}
