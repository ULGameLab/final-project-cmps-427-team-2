namespace FireChickenGames.Combat
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using FireChickenGames.Combat.Core;
    using FireChickenGames.Combat.Core.Integrations;
    using FireChickenGames.Combat.Core.WeaponConfiguration;
    using FireChickenGames.Combat.Core.WeaponManagement;
    using GameCreator.Core;
    using UnityEngine;
    using UnityEngine.Events;

    [AddComponentMenu("Fire Chicken Games/Combat/Weapon Stash")]
    public class WeaponStash : MonoBehaviour
    {
        #region Editor Fields
        [Tooltip("An object or variable with a Player/Character Shooter component.")]
        public TargetGameObject character = new TargetGameObject(TargetGameObject.Target.Player);

        [Tooltip("If the target is the player, the WeaponStashUi will be auto-wired if one is available in the scene.")]
        public WeaponStashUi weaponStashUi;

        [Header("Equipped")]
        public EquippableWeapon equippedWeapon;
        public EquippableWeapon EquippedWeapon
        {
            get { return equippedWeapon; }
            set { equippedWeapon = value; }
        }

        [Header("Inventory")]
        public List<EquippableWeapon> weapons = new List<EquippableWeapon>();
        #endregion

        protected ICharacterShooter characterShooter;
        public ICharacterShooter CharacterShooter {
            get {
                if (characterShooter == null)
                    InitializeCharacterShooter();
                return characterShooter;
            }
        }
        public bool IsArmedWithShooterWeapon => IsShooterWeapon(EquippedWeapon?.weapon);
        public bool IsAutoShooting => characterShooter != null && characterShooter.IsAutoShooting;
        public bool CatAttackWithShooterWeapon => IsArmedWithShooterWeapon && (characterShooter.IsAiming || EquippedWeapon.IsHipFireTypeShoot);
        public bool IsShooterFreeToAct => characterShooter != null && characterShooter.IsFreeToAct;
        public bool IsAmmoAimable => IsArmedWithShooterWeapon && EquippedWeapon.IsAmmoAimable;
        public bool IsAiming => IsArmedWithShooterWeapon && characterShooter.IsAiming;

        protected ICharacterMelee characterMelee;
        public bool IsArmedWithMeleeWeapon => IsMeleeWeapon(EquippedWeapon?.weapon);

        public class WeaponStashEvent : UnityEvent<ScriptableObject, ScriptableObject> { }
        public WeaponStashEvent weaponChangedEvent = new WeaponStashEvent();

        void Start()
        {
            if (character != null)
            {
                if (characterShooter == null)
                    InitializeCharacterShooter();
                if (characterShooter != null)
                    characterShooter.AddEventChangeAmmoListener();

                characterMelee = MeleeIntegrationManager.MakeCharacterMelee(character, gameObject);

                if (weaponStashUi == null && character.GetGameObject(gameObject).CompareTag("Player"))
                    weaponStashUi = HookManager.GetWeaponStashUi();

                UpdateStashedWeaponUi(EquippedWeapon);

                if (EquippedWeapon != null && characterShooter != null)
                    characterShooter.ChangeAmmo(EquippedWeapon.GetFirstAmmo());
            }

            if (weaponStashUi != null && characterShooter != null)
            {
                characterShooter.WeaponStashUi = weaponStashUi;
                characterShooter.AddEventChangeClipListener(weaponStashUi.SetAmmoInClip);
                characterShooter.AddEventChangeStorageListener(weaponStashUi.SetAmmoInStorage);
            }
        }

        void OnDestroy()
        {
            if (characterShooter == null)
                return;

            characterShooter.RemoveEventChangeAmmoListener();
            if (weaponStashUi != null)
            {
                characterShooter.RemoveEventChangeClipListener(weaponStashUi.SetAmmoInClip);
                characterShooter.RemoveEventChangeStorageListener(weaponStashUi.SetAmmoInStorage);
            }
        }

        public void InitializeCharacterShooter()
        {
            characterShooter = ShooterIntegrationManager.MakeCharacterShooter(character, gameObject, OnChangeShooterWeapon, weaponStashUi);
        }

        EquippableWeapon GetFirstOrLastWeapon(bool isReverseSelection = false)
        {
            return isReverseSelection ? weapons.LastOrDefault() : weapons.FirstOrDefault();
        }

        protected void OnChangeShooterWeapon(ScriptableObject weapon)
        {
            UpdateStashedWeaponUi(equippedWeapon);
        }

        protected void SetEquippedWeapon(EquippableWeapon equippableWeapon, bool shouldUpdateUi = true)
        {
            EquippedWeapon = equippableWeapon;
            if (shouldUpdateUi)
                UpdateStashedWeaponUi(equippedWeapon);
        }

        protected void UpdateStashedWeaponUi(EquippableWeapon equippableWeapon)
        {
            if (weaponStashUi == null || equippableWeapon == null)
                return;

            string weaponName;
            string weaponDescription;

            if (IsShooterWeapon(equippableWeapon.weapon))
                characterShooter.SetWeaponNameAndDescription(equippableWeapon.weapon);
            else if (IsMeleeWeapon(equippableWeapon.weapon))
            {
                weaponName = characterMelee.GetWeaponName(equippableWeapon.weapon);
                weaponDescription = characterMelee.GetWeaponDescription(equippableWeapon.weapon);
                weaponStashUi.RemoveAmmoClipAndStorage();
                weaponStashUi.SetWeapon(weaponName, weaponDescription);
            }

            UpdateFireModeDisplayName();
        }

        protected void UpdateFireModeDisplayName()
        {
            if (weaponStashUi != null)
                weaponStashUi.SetFireMode(EquippedWeapon.GetFireModeDisplayName());
        }

        /**
         * Public API
         */
        public bool HasWeaponDrawn()
        {
            return characterShooter?.CurrentWeapon != null || characterMelee?.CurrentWeapon != null;
        }

        public bool CanChangeWeapon()
        {
            return weapons.Any() && characterShooter != null && characterShooter.IsFreeToAct;
        }

        public IEnumerator CycleToNextWeapon(bool isReverseSelection = false)
        {
            if (!CanChangeWeapon())
                yield break;

            EquippableWeapon equippableWeapon = null;
            var weaponIndex = weapons.FindIndex(x => x.weapon == EquippedWeapon.weapon);
            
            if (weaponIndex == -1)
                equippableWeapon = GetFirstOrLastWeapon(isReverseSelection);
            else if (isReverseSelection && weaponIndex == 0)
                equippableWeapon = weapons.LastOrDefault();
            else if (isReverseSelection)
                equippableWeapon = weapons[weaponIndex - 1];
            else if (!isReverseSelection && weaponIndex == weapons.Count - 1)
                equippableWeapon = weapons.FirstOrDefault();
            else if (!isReverseSelection)
                equippableWeapon = weapons[weaponIndex + 1];

            if (equippableWeapon == null || equippableWeapon.weapon == null)
                yield break;

            if (IsShooterWeapon(equippableWeapon.weapon))
            {
                if (HasWeaponDrawn() && characterShooter.CurrentWeapon != equippableWeapon.weapon)
                {
                    /**
                     * If a weapon is drawn, draw a different weapon (and change ammo).
                     * The stashed weapon and ammo are changed after the weapon is drawn.
                     */
                    yield return characterMelee?.Sheathe();
                    SetEquippedWeapon(equippableWeapon, false);
                    yield return characterShooter.ChangeWeapon(equippableWeapon.weapon, equippableWeapon.ammo);
                }
                else
                {
                    /**
                     * If a weapon is not drawn, change the equipped weapon and ammo.
                     */
                    yield return characterMelee?.Sheathe();
                    SetEquippedWeapon(equippableWeapon);
                    characterShooter.ChangeAmmo(equippableWeapon.ammo);
                }
            }
            else if (IsMeleeWeapon(equippableWeapon.weapon))
            {
                if (HasWeaponDrawn() && characterMelee.CurrentWeapon != equippableWeapon.weapon)
                {
                    /**
                     * If a weapon is drawn, draw a different weapon (and change ammo).
                     * The equipped weapon is changed after the weapon is drawn.
                     */
                    yield return characterShooter?.ChangeWeapon(null);
                    SetEquippedWeapon(equippableWeapon);
                    yield return characterMelee?.Draw(equippableWeapon.weapon);
                }
                else
                {
                    /**
                     * If a weapon is not drawn, change the stashed weapon.
                     */
                    SetEquippedWeapon(equippableWeapon);
                    if (weaponStashUi != null)
                        weaponStashUi.RemoveAmmoClipAndStorage();
                }
            }
            yield return 0;
        }

        public IEnumerator CycleToNextAmmo(bool reverseSelection)
        {
            if (characterShooter == null || !characterShooter.IsFreeToAct)
                yield break;

            if (EquippedWeapon.CycleToNextAmmo(reverseSelection))
            {
                characterShooter.ChangeAmmo(EquippedWeapon.ammo);
                if (HasWeaponDrawn())
                    yield return characterShooter.Reload();
            }
            yield return 0;
        }

        public IEnumerator CycleToNextFireMode(bool reverseSelection)
        {
            if (characterShooter == null || !characterShooter.IsFreeToAct)
                yield break;

            if (EquippedWeapon.CycleToNextFireMode(reverseSelection))
                UpdateFireModeDisplayName();
            yield return 0;
        }

        public IEnumerator GiveWeapon(ScriptableObject weapon, ScriptableObject ammo = null, WeaponSettings weaponSettings = null)
        {
            if (weapon == null)
                yield break;

            var indexId = weapons.FindIndex(x => x.instanceId == weapon.GetInstanceID());
            if (indexId != -1)
                yield break;

            var equippableWeapon = new EquippableWeapon
            {
                instanceId = weapon.GetInstanceID(),
                weapon = weapon,
                ammo = ammo,
                weaponSettings = weaponSettings,
            };
            equippableWeapon.Initialize();

            weapons.Add(equippableWeapon);

            if (EquippedWeapon?.weapon == null)
            {
                EquippedWeapon = equippableWeapon;
                UpdateStashedWeaponUi(EquippedWeapon);
            }

            yield return 0;
        }

        public IEnumerator TakeWeapon(ScriptableObject weapon)
        {
            if (weapons == null)
                yield break;

            var weaponsInStash = weapons.Count();
            if (weaponsInStash == 0)
                yield break;

            // Holster/sheathe currnent weapon.
            if (HasWeaponDrawn())
            {
                yield return characterShooter?.ChangeWeapon(null);
                yield return characterMelee?.Sheathe();
            }

            // Switch to next weapon.
            if (weaponsInStash > 1)
                yield return CycleToNextWeapon();
            else
            {
                EquippedWeapon = null;
                if (weaponStashUi != null)
                {
                    weaponStashUi.SetWeapon("", "");
                    weaponStashUi.RemoveAmmoClipAndStorage();
                }
            }

            // Remove weapon.
            weapons.RemoveAll(x => x.instanceId == weapon.GetInstanceID());

            yield return 0;
        }

        public IEnumerator TakeCurrentWeapon()
        {
            yield return TakeWeapon(EquippedWeapon.weapon);
        }

        public IEnumerator DrawShooter()
        {
            if (IsArmedWithShooterWeapon)
            {
                if (EquippedWeapon.ammo == null && EquippedWeapon.weapon != null)
                    EquippedWeapon.ammo = EquippedWeapon.GetFirstAmmo();

                weaponChangedEvent.Invoke(EquippedWeapon.weapon, EquippedWeapon.ammo);

                yield return characterShooter.ChangeWeapon(EquippedWeapon.weapon, EquippedWeapon.ammo);
            }
            yield return 0;
        }

        public IEnumerator ToggleDrawnShooterWeapon()
        {
            if (!IsArmedWithShooterWeapon)
                yield break;

            if (HasWeaponDrawn())
                yield return characterShooter.ChangeWeapon(null, null);
            else
                yield return DrawShooter();
            yield return 0;
        }

        public IEnumerator DrawMelee()
        {
            yield return characterMelee?.Draw(EquippedWeapon.weapon);
        }

        public IEnumerator ToggleDrawnMeleeWeapon()
        {
            if (!IsArmedWithMeleeWeapon)
                yield break;

            if (HasWeaponDrawn())
                yield return characterMelee.Sheathe();
            else
                yield return DrawMelee();
            yield return 0;
        }

        public IEnumerator Attack()
        {
            if (IsArmedWithShooterWeapon && (characterShooter.IsAiming || EquippedWeapon.IsHipFireTypeShoot))
                yield return characterShooter.Shoot(EquippedWeapon);
            if (IsArmedWithMeleeWeapon)
                characterMelee.Attack(EquippedWeapon);
            yield return 0;
        }

        public IEnumerator StopAttacking()
        {
            if (IsArmedWithShooterWeapon)
                yield return characterShooter.StopAutoShooting();
            yield return 0;
        }

        public void StopAttackingInstant()
        {
            if (IsArmedWithShooterWeapon)
                characterShooter.StopAutoShootingInstant();
        }

        public void StartChargedShot()
        {
            if (!IsArmedWithShooterWeapon || !characterShooter.IsChargeShotRequired(EquippedWeapon.ammo))
                return;

            if (!characterShooter.IsAiming && EquippedWeapon.IsHipFireTypeShoot)
                characterShooter.StartAiming();

            if (characterShooter.IsAiming || EquippedWeapon.IsHipFireTypeShoot)
                characterShooter.StartChargedShot();
        }

        public void ExecuteChargedShot()
        {
            if (!IsArmedWithShooterWeapon)
                return;

            characterShooter.ExecuteChargedShot();
        }

        public bool IsShooterWeapon(ScriptableObject weapon)
        {
            return characterShooter != null && characterShooter.IsShooterWeapon(weapon);
        }

        public bool IsMeleeWeapon(ScriptableObject weapon)
        {
            return characterMelee != null && characterMelee.IsMeleeWeapon(weapon);
        }

        public IEnumerator Reload()
        {
            if (IsArmedWithShooterWeapon)
                yield return characterShooter.Reload();
            yield return 0;
        }

        public void StartAiming()
        {
            if (IsArmedWithShooterWeapon && !characterShooter.IsAiming)
                characterShooter.StartAiming();
        }

        public void StopAiming()
        {
            if (IsArmedWithShooterWeapon && characterShooter.IsAiming)
                characterShooter.StopAiming();
        }

        public void StopBlocking()
        {
            if (IsArmedWithMeleeWeapon && !characterMelee.IsBlocking)
                characterMelee.StartBlocking();
        }

        public void StartBlocking()
        {
            if (IsArmedWithMeleeWeapon && characterMelee.IsBlocking)
                characterMelee.StopBlocking();
        }

        public void SetMeleeInvincibility(float duration)
        {
            if (characterMelee != null)
                characterMelee.SetInvincibility(duration);
        }
    }
}
