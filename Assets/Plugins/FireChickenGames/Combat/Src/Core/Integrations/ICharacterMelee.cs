namespace FireChickenGames.Combat.Core.Integrations
{
    using System.Collections;
    using FireChickenGames.Combat.Core.WeaponManagement;
    using UnityEngine;

    public interface ICharacterMelee
    {
        ScriptableObject CurrentWeapon { get; }
        ScriptableObject CurrentShield { get; }
        bool IsBlocking { get; }

        void SetCharacterMelee(Component characterMelee);

        IEnumerator Sheathe();
        IEnumerator Draw(ScriptableObject weapon, ScriptableObject shield = null);
        void TakeWeapon();
        string GetWeaponName(ScriptableObject stashedWeapon);
        string GetWeaponDescription(ScriptableObject stashedWeapon);
        bool IsMeleeWeapon(ScriptableObject weapon);
        void Attack(EquippableWeapon equippedWeapon);
        void StartBlocking();
        void StopBlocking();
        void SetInvincibility(float duration);
    }
}
