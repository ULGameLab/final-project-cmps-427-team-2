namespace FireChickenGames.Combat
{
    using FireChickenGames.Combat.Core.Cameras;
    using FireChickenGames.Combat.Core.PlayerInput;
    using GameCreator.Characters;
    using GameCreator.Core;
    using UnityEngine;

    [AddComponentMenu("Fire Chicken Games/Combat/Attacker")]
    public class Attacker : MonoBehaviour
    {
        #region Editor Fields
        public TargetGameObject weaponStashGameObject = new TargetGameObject(TargetGameObject.Target.Player);

        // Shooter
        public UserInputKey shooterAttackKey = new UserInputKey(MouseButton.Left);
        public UserInputKey aimKey = new UserInputKey(MouseButton.Right);
        public bool canRunWhileAiming = false;
        public bool zoomInWhileAiming = true;
        public float zoomInAimingFieldOfView = 40f;
        public float zoomInAimingTransitionDurationInSeconds = 0.5f;

        public float zoomOutAimingFieldOfView = 40f;
        public float zoomOutAimingTransitionDurationInSeconds = 0.5f;

        public UserInputKey drawWeaponKey = new UserInputKey(KeyCode.E);
        public UserInputKey cycleAmmoKey = new UserInputKey(KeyCode.B);
        public UserInputKey cycleFireModeKey = new UserInputKey(KeyCode.N);
        public UserInputKey cycleWeaponKey = new UserInputKey(KeyCode.Tab);
        public UserInputKey reloadAmmoKey = new UserInputKey(KeyCode.R);
        public UserInputKey dropWeaponKey = new UserInputKey(KeyCode.T);

        // Melee
        public UserInputKey meleeAttackKey = new UserInputKey(MouseButton.Left);
        public UserInputKey unsheatheWeaponKey = new UserInputKey(KeyCode.E);
        public UserInputKey blockKey = new UserInputKey(MouseButton.Right);
        public UserInputKey evadeKey = new UserInputKey(KeyCode.LeftShift);
        public float evasionInvincibilityDuration = 0.5f;
        public Actions actionCharacterDash;
        #endregion

        #region Private Fields
        private Targeter targeter;
        private WeaponStash weaponStash;
        private Character character;
        #endregion

        void Start()
        {
            var weaponStashGo = weaponStashGameObject.GetGameObject(gameObject);
            if (weaponStash == null && !weaponStashGo.TryGetComponent(out weaponStash))
                weaponStash = weaponStashGo.AddComponent<WeaponStash>();

            if (character == null && weaponStashGo != null)
                weaponStashGo.TryGetComponent(out character);
        }

        void Update()
        {
            if (weaponStash.IsShooterFreeToAct && weaponStash.IsArmedWithShooterWeapon && !UpdateDropWeapon())
                UpdateShooterUserInput();

            else if (weaponStash.IsArmedWithMeleeWeapon && !UpdateDropWeapon())
                UpdateMeleeUserInput();
        }

        protected bool UpdateDropWeapon()
        {
            if (dropWeaponKey.IsDown)
            {
                StartCoroutine(weaponStash.TakeCurrentWeapon());
                return true;
            }
            return false;
        }

        protected void UpdateShooterUserInput()
        {
            // Draw and Holster.
            if (drawWeaponKey.IsDown)
                StartCoroutine(weaponStash.ToggleDrawnShooterWeapon());
            
            // Start Aiming.
            if (weaponStash.IsAmmoAimable)
            {
                if (aimKey.IsDown)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    weaponStash.StartAiming();
                    SetCharacterCanRun(false);
                    ZoomInFieldOfView();
                }
                else if (aimKey.IsUp)
                {
                    ZoomOutFieldOfView();
                    Cursor.lockState = CursorLockMode.None;
                    weaponStash.StopAiming();
                    SetCharacterCanRun(true);
                }
            }

            // Stop auto attacking.
            if (!shooterAttackKey.IsHeld && weaponStash.IsAutoShooting)
                StartCoroutine(weaponStash.StopAttacking());

            // Attacking.
            if (shooterAttackKey.IsDown)
            {
                weaponStash.StartChargedShot();
                StartCoroutine(weaponStash.Attack());
            }
            if (shooterAttackKey.IsUp)
            {
                weaponStash.StopAttackingInstant();
                weaponStash.ExecuteChargedShot();
            }

            // Weapon Management
            if (cycleAmmoKey.IsDown)
                StartCoroutine(weaponStash.CycleToNextAmmo(false));
            else if (cycleFireModeKey.IsDown)
                StartCoroutine(weaponStash.CycleToNextFireMode(false));
            else if (cycleWeaponKey.IsDown)
                StartCoroutine(weaponStash.CycleToNextWeapon(false));
            else if (reloadAmmoKey.IsDown)
                StartCoroutine(weaponStash.Reload());
        }

        protected void UpdateMeleeUserInput()
        {
            // Draw and Holster.
            if (unsheatheWeaponKey.IsDown)
                StartCoroutine(weaponStash.ToggleDrawnMeleeWeapon());

            if (blockKey.IsDown)
                weaponStash.StartBlocking();
            if (blockKey.IsUp)
                weaponStash.StopBlocking();

            if (meleeAttackKey.IsDown)
                StartCoroutine(weaponStash.Attack());

            if (evadeKey.IsDown)
            {
                weaponStash.SetMeleeInvincibility(evasionInvincibilityDuration);
                if (actionCharacterDash != null)
                    actionCharacterDash.ExecuteWithTarget(weaponStashGameObject.GetGameObject(gameObject));
            }
        }

        protected void SetCharacterCanRun(bool canRun)
        {
            if (!canRunWhileAiming && character != null)
                character.characterLocomotion.canRun = canRun;
        }

        protected void ZoomInFieldOfView()
        {
            if (!zoomInWhileAiming)
                return;
            StartCoroutine(CameraManager.ChangeFieldOfView(zoomInAimingFieldOfView, zoomInAimingTransitionDurationInSeconds));
        }

        protected void ZoomOutFieldOfView()
        {
            if (zoomInWhileAiming)
                StartCoroutine(CameraManager.ChangeFieldOfView(zoomOutAimingFieldOfView, zoomOutAimingTransitionDurationInSeconds));
        }
    }
}
