namespace FireChickenGames.ShooterCombat.Core.Integrations
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using FireChickenGames.Combat;
    using FireChickenGames.Combat.Core.Integrations;
    using FireChickenGames.Combat.Core.WeaponConfiguration.Shooter;
    using FireChickenGames.Combat.Core.WeaponManagement;
    using FireChickenGames.ShooterCombat.Core.Aiming;
    using GameCreator.Characters;
    using GameCreator.Shooter;
    using UnityEngine;
    using UnityEngine.Events;

    public class CharacterShooterAdapter : ICharacterShooter
    {
        public Character Character { get { return characterShooter != null ? characterShooter.character : null; } }
        public CharacterShooter characterShooter;

        public bool IsFreeToAct => IsControllable && !IsCharacterLocomotionBusy && !IsDrawing && !IsHolstering && !IsReloading;
        public bool IsControllable => Character != null && Character.IsControllable();
        public bool IsCharacterLocomotionBusy => Character != null && Character.characterLocomotion.isBusy;

        public bool IsDrawing => characterShooter != null && characterShooter.isDrawing;
        public bool IsHolstering => characterShooter != null && characterShooter.isHolstering;
        public bool IsReloading => characterShooter != null && characterShooter.isReloading;
        private bool isShootingWithoutAiming = false;
        public bool IsAiming => characterShooter != null && characterShooter.isAiming;
        public bool IsChargingShot => characterShooter != null && characterShooter.isChargingShot;

        public ScriptableObject CurrentWeapon => characterShooter.currentWeapon;
        public ScriptableObject CurrentAmmo => characterShooter.currentAmmo;

        public WeaponStashUi WeaponStashUi { get; set; }
        public UnityAction<ScriptableObject> SetStashedWeapon { get; set; }

        // Aim Assist
        private RaycastHit[] bufferCastHits = new RaycastHit[100];
        private class AimAssistRaycastComparer : IComparer<RaycastHit>
        {
            public int Compare(RaycastHit x, RaycastHit y)
            {
                return x.distance.CompareTo(y.distance);
            }
        }
        private static readonly AimAssistRaycastComparer AIM_ASSIST_RAYCAST_COMPARER = new AimAssistRaycastComparer();

        // Fire Mode
        private enum AutoShootingState
        {
            Stopped,
            Stopping,
            Started,
        }
        private AutoShootingState autoShootingState = AutoShootingState.Stopped;
        public bool IsAutoShooting => autoShootingState == AutoShootingState.Started;
        
        public void OnChangeAmmo(Ammo ammo)
        {
            SetStashedWeapon.Invoke(CurrentWeapon);
            if (WeaponStashUi == null || ammo == null)
                return;
            
            WeaponStashUi.SetAmmoInClip(ammo.ammoID, characterShooter.GetAmmoInClip(ammo.ammoID));
            WeaponStashUi.SetAmmoInStorage(ammo.ammoID, characterShooter.GetAmmoInStorage(ammo.ammoID));
            WeaponStashUi.SetAmmoMaxClipText(ammo.clipSize.ToString());
            SetAmmoNameAndDescription(ammo);
        }

        public void SetCharacterShooter(Component characterShooter)
        {
            this.characterShooter = characterShooter as CharacterShooter;
        }

        public bool HasCharacterShooter()
        {
            return characterShooter != null;
        }

        public Component GetCharacterShooter()
        {
            return characterShooter;
        }

        public void AddEventOnAimListener(UnityAction<bool> onAim)
        {
            if (characterShooter != null)
                characterShooter.eventOnAim.AddListener(onAim);
        }

        public void RemoveEventOnAimListener(UnityAction<bool> onAim)
        {
            if (characterShooter != null)
                characterShooter.eventOnAim.RemoveListener(onAim);
        }

        public void StartAiming(IAimingAtProximityTarget aimingAtTarget = null, bool isCrosshairVisible = true)
        {
            if (aimingAtTarget != null)
            {
                var baseAimingAtTarget = (AimingAtProximityTarget)aimingAtTarget;
                characterShooter.StartAiming(baseAimingAtTarget);
            }
            else
                characterShooter.StartAiming(new AimingCameraDirection(characterShooter));

            if (!isCrosshairVisible)
                DestroyCrosshair();
        }

        public void StopAiming()
        {
            characterShooter.StopAiming();
        }

        public void DestroyCrosshair()
        {
            WeaponCrosshair.Destroy();
        }

        public IEnumerator ChangeWeapon(ScriptableObject weapon = null, ScriptableObject ammo = null)
        {
            yield return characterShooter.ChangeWeapon(weapon as Weapon, ammo as Ammo);
        }

        public void SetWeaponNameAndDescription(ScriptableObject weapon)
        {
            if (WeaponStashUi == null || !IsShooterWeapon(weapon))
                return;

            var shooterWeapon = weapon as Weapon;
            WeaponStashUi.SetWeapon(shooterWeapon.weaponName.GetText(), shooterWeapon.weaponDesc.GetText());
        }

        public void ChangeAmmo(ScriptableObject ammo)
        {
            if (ammo is Ammo)
            {
                var isAiming = IsAiming;
                var aimingBase = characterShooter.aiming;
                if (isAiming)
                    characterShooter.StopAiming();
                characterShooter.ChangeAmmo(ammo as Ammo);
                if (isAiming && aimingBase != null)
                    characterShooter.StartAiming(aimingBase);
                SetAmmoNameAndDescription(ammo);
            }
        }

        public void SetAmmoNameAndDescription(ScriptableObject ammo)
        {
            if (!(ammo is Ammo))
                return;

            var shooterAmmo = ammo as Ammo;

            if (WeaponStashUi != null)
                WeaponStashUi.SetAmmo(shooterAmmo.ammoName.GetText(), shooterAmmo.ammoDesc.GetText());
        }

        public bool IsShooterWeapon(ScriptableObject weapon)
        {
            return weapon is Weapon;
        }

        public void SetAmmoNameAndDescriptionFromWeapon(ScriptableObject weapon)
        {
            if (WeaponStashUi == null || !IsShooterWeapon(weapon))
                return;

            SetAmmoNameAndDescription((weapon as Weapon).defaultAmmo);
        }

        public bool IsClipEmpty()
        {
            return characterShooter.GetAmmoInClip(characterShooter.currentAmmo.ammoID) == 0;
        }

        public IEnumerator Reload()
        {
            if (CurrentWeapon == null || CurrentAmmo == null || !IsFreeToAct)
                yield break;

            var currentClip = characterShooter.GetAmmoInClip(characterShooter.currentAmmo.ammoID);
            var currentStorage = characterShooter.GetAmmoInStorage(characterShooter.currentAmmo.ammoID);
            if (currentStorage <= 0 || currentClip >= characterShooter.currentAmmo.clipSize)
            {
                // Just handle th ereload-related animations without adding ammunition to the clip.
                if (characterShooter.isChargingShot)
                    characterShooter.currentAmmo.StopCharge(characterShooter);
                yield return characterShooter.currentAmmo.Reload(characterShooter);
            }
            else
                // Do a full clip reload (animation plus add to clip from storage).
                yield return characterShooter.Reload();
        }

        public bool IsChargeShotRequired(ScriptableObject ammo)
        {
            return ammo is Ammo ammoInstance && ammoInstance.chargeType != Ammo.TriggerType.DisableCharge;
        }

        public void StartChargedShot()
        {
            characterShooter.StartChargedShot();
        }

        public void ExecuteChargedShot()
        {
            if (characterShooter.currentAmmo == null)
                return;

            characterShooter.ExecuteChargedShot();

            if (characterShooter.currentAmmo.chargeType != Ammo.TriggerType.DisableCharge)
                characterShooter.StopAiming();
        }

        public IEnumerator Shoot(EquippableWeapon equippableWeapon)
        {
            if (characterShooter == null || characterShooter.currentAmmo == null)
                yield break;

            if (IsAiming)
                yield return ShootWithFireMode(equippableWeapon);
            else
            {
                isShootingWithoutAiming = true;
                float startTime = Time.time + 0.1f;
                var waitUntilBefore = new WaitUntil(() => Time.time > startTime);
                float stopTime = Time.time + 0.2f;
                var waitUntilAfter = new WaitUntil(() => Time.time > stopTime);

                StartAiming();
                DestroyCrosshair();
                yield return waitUntilBefore;
                yield return ShootWithFireMode(equippableWeapon);
                yield return waitUntilAfter;
                StopAiming();
                isShootingWithoutAiming = false;
            }
        }

        public IEnumerator ShootWithFireMode(EquippableWeapon equippableWeapon)
        {
            if (characterShooter == null || characterShooter.currentAmmo == null)
                yield break;

            var fireMode = equippableWeapon.GetFireMode();
            if (fireMode == FireMode.Burst)
                yield return ShootBurst(equippableWeapon.GetBurstAmount());
            else if (fireMode == FireMode.Auto)
                yield return ShootAuto();
            else
                yield return ShootSingle();

            yield return 0;
        }

        public IEnumerator ShootSingle()
        {
            characterShooter.Shoot();
            yield return 0;
        }

        public IEnumerator ShootBurst(int burstAmount)
        {
            int burstCount = Mathf.Min(characterShooter.GetAmmoInClip(characterShooter.currentAmmo.ammoID), burstAmount);
            if (burstCount <= 0)
                burstCount = 1;

            var fireRate = characterShooter.currentAmmo.fireRate.GetInt(characterShooter.gameObject);
            for (int i = 0; i < burstCount; ++i)
            {
                while (!CanShootFireRate(fireRate))
                    yield return null;

                characterShooter.Shoot();
            }
        }

        public IEnumerator ShootAuto()
        {
            if (autoShootingState == AutoShootingState.Stopping)
            {
                autoShootingState = AutoShootingState.Stopped;
                yield break;
            }
            var fireRate = characterShooter.currentAmmo.fireRate.GetInt(characterShooter.gameObject);
            autoShootingState = AutoShootingState.Started;
            while (autoShootingState == AutoShootingState.Started)
            {
                while (!CanShootFireRate(fireRate))
                    yield return null;

                characterShooter.Shoot();

                if (IsClipEmpty())
                    autoShootingState = AutoShootingState.Stopped;
            }
        }

        public bool CanShootFireRate(int fireRate)
        {
            return characterShooter.CanShootFireRate(characterShooter.currentAmmo.ammoID, fireRate);
        }

        public IEnumerator StopAutoShooting()
        {
            autoShootingState = AutoShootingState.Stopping;
            yield return 0;
        }

        public void StopAutoShootingInstant()
        {
            autoShootingState = AutoShootingState.Stopped;
        }

        #region Events Handlers
        public void AddEventChangeAmmoListener()
        {
            characterShooter.eventChangeAmmo.AddListener(OnChangeAmmo);
        }

        public void RemoveEventChangeAmmoListener()
        {
            characterShooter.eventChangeAmmo.RemoveListener(OnChangeAmmo);
        }

        public void AddEventChangeClipListener(UnityAction<string, int> setAmmoInClip)
        {
            characterShooter.AddListenerClipChange(setAmmoInClip);
        }

        public void RemoveEventChangeClipListener(UnityAction<string, int> setAmmoInClip)
        {
            characterShooter.RmvListenerClipChange(setAmmoInClip);
        }

        public void AddEventChangeStorageListener(UnityAction<string, int> setAmmoInStorage)
        {
            characterShooter.AddListenerStorageChange(setAmmoInStorage);
        }

        public void RemoveEventChangeStorageListener(UnityAction<string, int> setAmmoInStorage)
        {
            characterShooter.RmvListenerStorageChange(setAmmoInStorage);
        }
        #endregion

        public GameObject GetAimAssistTarget()
        {
            if (isShootingWithoutAiming)    
                return null;

            if (characterShooter.currentAmmo == null || characterShooter.aiming == null)
                return null;

            var ammo = characterShooter.currentAmmo;

            if (ammo.shootType != Ammo.ShootType.SphereCast && ammo.shootType != Ammo.ShootType.SphereCastAll)
                return null;

            var shootPositionTarget = characterShooter.aiming.GetAimingPosition();
            var shootPositionRaycast = characterShooter.aiming.pointShootingRaycast;
            var direction = (shootPositionTarget - shootPositionRaycast).normalized;

            var hitCounter = Physics.SphereCastNonAlloc(
                shootPositionRaycast,
                ammo.radius,
                direction,
                bufferCastHits,
                ammo.distance,
                ammo.layerMask,
                ammo.triggersMask
            );

            if (hitCounter == 0)
                return null;

            var maxCount = Mathf.Min(hitCounter, this.bufferCastHits.Length);
            Array.Sort(bufferCastHits, 0, maxCount, AIM_ASSIST_RAYCAST_COMPARER);

            GameObject aimAssistTarget = null;
            if (bufferCastHits.Length > 0)
            {
                // Get all possible targets that aren't the shooter.
                var possibleTargets = bufferCastHits.ToList()
                    .Where(hit => {
                        return hit.collider != null &&
                               hit.collider.gameObject != null &&
                               hit.collider.gameObject != Character.gameObject &&
                               hit.collider.gameObject.TryGetComponent(out Targetable targetable);
                    })
                    .OrderBy(x => Vector3.Distance(shootPositionRaycast, x.collider.gameObject.transform.position));

                if (possibleTargets.Any())
                    aimAssistTarget = possibleTargets.FirstOrDefault().collider.gameObject;

            }
            Array.Clear(bufferCastHits, 0, bufferCastHits.Length);
            return aimAssistTarget;
        }
    }
}
