namespace FireChickenGames.Combat
{
    using System.Collections.Generic;
    using System.Linq;
    using FireChickenGames.Combat.Core;
    using FireChickenGames.Combat.Core.Cameras;
    using FireChickenGames.Combat.Core.Integrations;
    using FireChickenGames.Combat.Core.Targeting;
    using FireChickenGames.Combat.Core.TargetingStrategies;
    using GameCreator.Camera;
    using GameCreator.Core;
    using GameCreator.Variables;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [AddComponentMenu("Fire Chicken Games/Combat/Targeter")]
    public class Targeter : MonoBehaviour
    {
        // Public (hidden from inspector)
        public const string TARGETING_TYPE_PROXIMITY = "Proximity";
        public const string TARGETING_TYPE_MOUSE = "Mouse";
        public const string TARGETING_TYPE_AIM_ASSIST = "AimAssist";

        // Public
        [Tooltip("The GameObject that targets.")]
        public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.Player);
        [Tooltip("The GameObject with a weapon stash component.")]
        public TargetGameObject weaponStashGameObject = new TargetGameObject(TargetGameObject.Target.Player);

        [Tooltip("An optional variable to access the Targeter's current target.")]
        [VariableFilter(Variable.DataType.GameObject)]
        public VariableProperty currentTargetVariable = new VariableProperty(Variable.VarType.GlobalVariable);

        [Tooltip("The number of seconds between triggering the Targetable's \"Ongoing Focus\" event.")]
        [Range(0, 3600)]
        public float broadcastContinuingToBeTargetedInSeconds = 5f;

        // Aiming
        [Tooltip("Causes the character to point their weapon at the target while locked onto it.")]
        public bool autoAimAtTarget = true;

        [Tooltip("The camera motor to use while aiming and not focused on a target.")]
        public CameraMotor aimingCameraMotor;
        [Tooltip("The number of seconds it takes to switch to the aiming camera motor.")]
        public float startAimingTransitionDuration = 0.1f;
        [Tooltip("The number of seconds it takes to switch to back to the aiming camera motor.")]
        public float stopAimingTransitionDuration = 0.2f;

        // Targeting
        [Tooltip("Determines the overall style of targeting.")]
        public TargetingType targetingType = TargetingType.Proximity;

        [Tooltip("The amount of horizontal movement it takes to deselect an aim assist target.")]
        [Range(0f, 5f)]
        public float aimAssistDeselectTargetTolerance = 0f;

        [Tooltip("Seconds to wait before a new target can be acquired.")]
        [Range(0f, 5f)]
        public float aimAssistAcquireTargetTimerCooldown = 0f;

        [Tooltip("If enabled, targeting will be enabled when the scene loads.")]
        public bool isTargetingEnabled;

        [Tooltip("If enabled, the closest target will be automatically set if one is available.")]
        public bool isAcquireInitialTargetEnabled = true;

        [Tooltip("If set, the camera will switch to this alternative camera motor while targeting a target.")]
        public CameraMotor targetingCameraMotor;
        [Tooltip("The number of seconds it takes to switch to the targeting camera motor.")]
        public float startTargetingTransitionDuration = 0.1f;
        [Tooltip("The number of seconds it takes to switch to back to the main camera motor.")]

        public float stopTargetingTransitionDuration = 0.2f;

        [Tooltip("If enabled, only targets possibly visible by the camera (i.e. in its view frustum) are targetable.")]
        public bool onlyTargetVisibleToCamera = true;
        
        [Tooltip("If enabled, targets hidden behind objects are not targetable. Note that this option is off by default because the player needs to be on a dedicated layer which requires manual configuration.")]
        public bool onlyTargetNonOccluded;

        [Tooltip("Layers to ignore when determining visibility, e.g. a \"Targetable\" layer that contains all objects with the Targetable component.")]
        public List<UnityLayer> layersToIgnoreForVisibilityOcclusion = new List<UnityLayer>();

        // Input
        [Tooltip("If enabled, use the Targeter component's built-in input controls.")]
        public bool useNativeInputControls = true;
        [Tooltip("Toggles targeting on/off.")]
        public KeyCode targetLockEnabledOnKeyUp = KeyCode.Q;
        [Tooltip("Selects the next available target.")]
        public KeyCode selectNextTargetOnKeyUp = KeyCode.C;
        [Tooltip("Selects the previous available target.")]
        public KeyCode selectPreviousTargetOnKeyUp = KeyCode.Z;

        // Private
        private GameObject targeterGameObject;
        private WeaponStash weaponStash;
        private SphereCollider sphereCollider;
        private float targetingRangeThresholdOffset = 1.0f;
        private float TotalTargetingRange { get { return sphereCollider.radius + targetingRangeThresholdOffset; } }
        private Dictionary<int, ProximityTarget> proximityTargets = new Dictionary<int, ProximityTarget>();
        private ICharacterShooter characterShooter;

        private ITargetingStrategy targetingStrategy;
        private IAimingAtProximityTarget aimingAtTarget;
        public ProximityTarget currentProximityTarget;

        private CameraController cameraController;
        private CameraMotor mainCameraMotor;
        private float aimAssistAcquireTargetTimer = 0.0f;

        private float broadcastContinuingToBeTargetedTimer = 0.0f;

        void Start()
        {
            if (targetGameObject == null)
                return;

            targeterGameObject = targetGameObject.GetGameObject(gameObject);

            if (targeterGameObject == null)
                return;

            if (!TryGetComponent(out sphereCollider))
            {
                sphereCollider = gameObject.AddComponent<SphereCollider>();
                sphereCollider.radius = 30f;
                sphereCollider.isTrigger = true;
            }
            if (!layersToIgnoreForVisibilityOcclusion.Any())
                layersToIgnoreForVisibilityOcclusion.Add(new UnityLayer(targeterGameObject.layer));

            /**
             * Get ShooterCombat module's CharacterShooter component and aiming object, if available.
             */
            var weaponStashGo = weaponStashGameObject.GetGameObject(gameObject);
            if (weaponStash != null || weaponStashGo.TryGetComponent(out weaponStash))
                characterShooter = weaponStash.CharacterShooter;

            if (characterShooter == null)
                characterShooter = ShooterIntegrationManager.MakeCharacterShooter(targetGameObject, targeterGameObject);
            characterShooter?.AddEventOnAimListener(OnAim);
            aimingAtTarget = ShooterIntegrationManager.MakeAimingAtProximityTargetOrDefault(characterShooter);

            /**
             * Get MeleeCombat module's targeting strategy, if available.
             */
            targetingStrategy = MeleeIntegrationManager.MakeMeleeTargetingStrategyOrDefault(targeterGameObject);

            /**
             * Get main camera controller for focused camera motor management.
             */
            var mainCamera = HookManager.GetCamera();
            if (mainCamera.TryGetComponent(out cameraController))
                mainCameraMotor = cameraController.currentCameraMotor;
        }

        void OnDestroy()
        {
            if (HasCharacterShooter())
                characterShooter.RemoveEventOnAimListener(OnAim);
        }

        private void OnAim(bool isAiming)
        {
            if (!isAiming)
                aimingAtTarget.UnsetTarget();
        }

        void OnTriggerExit(Collider targetCollider)
        {
            proximityTargets.Remove(targetCollider.gameObject.GetInstanceID());
        }

        void OnTriggerEnter(Collider targetCollider)
        {
            StartTrackingTargetable(targetCollider);
        }

        void OnTriggerStay(Collider targetCollider)
        {
            StartTrackingTargetable(targetCollider);
        }

        void StartTrackingTargetable(Collider targetCollider)
        {
            var targetableGameObjectId = targetCollider.gameObject.GetInstanceID();
            if (proximityTargets.ContainsKey(targetableGameObjectId))
                return;
            if (targetCollider.gameObject.TryGetComponent<Targetable>(out var targetable) && targetable.CanBeTargeted())
                proximityTargets[targetableGameObjectId] = new ProximityTarget(targetable);
        }

        void Update()
        {
            /**
             * Change Camera Motor
             */
            UpdateCameraMotor();

            /**
             * Toggle targeting on/off.
             */
            var isTargetingBecomingEnabled = false;
            if (useNativeInputControls && Input.GetKeyUp(targetLockEnabledOnKeyUp))
            {
                isTargetingEnabled = !isTargetingEnabled;
                isTargetingBecomingEnabled = isTargetingEnabled;
            }

            /**
              * Handle cases where current target is no longer targetable.
              */
            if (currentProximityTarget?.GameObject == null)
                // Targetable's parent gameObject has been destroyed.
                ReleaseTargetFocus();

            if (currentProximityTarget != null && !proximityTargets.ContainsKey(currentProximityTarget.Id))
                // Targetable is no longer tracked (can happen for a variety of reasons, depending on which features are enabled).
                ReleaseTargetFocus();

            if (currentProximityTarget != null && !currentProximityTarget.CanBeTargeted(TotalTargetingRange))
                // Targetable still might be tracked (until untracked in LateUpdate or OnTriggerExit), but is out of range.
                ReleaseTargetFocus();

            /**
             * Handle cases where there are no targets available at all (regardless of targeting visibility options).
             */
            if (!proximityTargets.Any() || !isTargetingEnabled)
            {
                ReleaseTargetFocus();
                return;
            }

            /**
             * Update proximity target distances.
             */
            foreach (var proximityTarget in proximityTargets)
                proximityTarget.Value.DistanceToTarget = Vector3.Distance(gameObject.transform.position, proximityTarget.Value.Position);

            /**
             * If only targeting objects in the view frustum, don't bother to continue if switching targeting on with no visible targets.
             */
            if (onlyTargetVisibleToCamera && isTargetingBecomingEnabled && !GetAvailableTargets().Any())
                return;


            if (IsProximityTargeter())
            {
                /**
                 * Handle case when current target is already selected... target switching, aiming transition, etc.
                 */
                if (useNativeInputControls)
                {
                    if (Input.GetKeyUp(selectNextTargetOnKeyUp))
                        CycleToNextTarget();
                    else if (Input.GetKeyUp(selectPreviousTargetOnKeyUp))
                        CycleToNextTarget(true);
                    else
                        SetInitialTarget();
                }
                else
                    SetInitialTarget();
            }
            else if (IsMouseTargeter())
            {
                /**
                 * Mouse Target Selection
                 */
                if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)
                {
                    var rayFromMouseClick = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(rayFromMouseClick, out var hit))
                    {
                        if (hit.transform.gameObject.TryGetComponent(out Targetable targetable))
                        {
                            if (IsCurrentTarget(targetable.gameObject))
                                ReleaseTargetFocus();
                            else if (targetable.isMouseTargetSelectionEnabled)
                                SetCurrentTarget(targetable.gameObject);
                        }
                        else
                            // No targetable (or UI element) clicked.
                            ReleaseTargetFocus();
                    }
                }
            }
            else if (IsAimAssistTargeter())
            {
                // Update target acquisition timer.
                if (!HasTarget() && aimAssistAcquireTargetTimer > 0.0f)
                    aimAssistAcquireTargetTimer -= Time.deltaTime;
                else if (HasTarget())
                    aimAssistAcquireTargetTimer = aimAssistAcquireTargetTimerCooldown;

                if (!HasCharacterShooter())
                    // This should never happen (because aim assist should not be selectable if there is no character shooter),
                    // but better to be defensive here.
                    return;

                if (characterShooter.IsAiming && !HasTarget() && aimAssistAcquireTargetTimer > 0f)
                    // Prevent Aim Assist from too quickly reacquiring a target after releasing target focus.
                    return;

                if (!characterShooter.IsAiming && HasTarget())
                    ReleaseTargetFocus();

                if (!characterShooter.IsAiming)
                    return;

                /**
                 * Aim Assist Target Selection
                 */
                var aimAssistTarget = characterShooter.GetAimAssistTarget();

                if (aimAssistTarget != null && !IsCurrentTarget(aimAssistTarget))
                    SetCurrentTarget(aimAssistTarget);

                var axisShift = Input.GetAxis("Mouse X");
                if (axisShift < -aimAssistDeselectTargetTolerance || axisShift > aimAssistDeselectTargetTolerance)
                    ReleaseTargetFocus();
            }

            broadcastContinuingToBeTargetedTimer += Time.deltaTime;
            if (HasTarget() && broadcastContinuingToBeTargetedTimer >= broadcastContinuingToBeTargetedInSeconds)
            {
                broadcastContinuingToBeTargetedTimer = 0f;
                BroadcastContinuingToBeTargeted(currentProximityTarget);
            }
        }

        void SetCurrentProximityTarget(ProximityTarget proximityTarget)
        {
            currentProximityTarget = proximityTarget;
            currentTargetVariable.Set(currentProximityTarget?.GameObject);
        }

        void SetInitialTarget(bool requireInitialTargetingToBeEnabled = true)
        {
            if (!isAcquireInitialTargetEnabled && requireInitialTargetingToBeEnabled)
                return;

            if (currentProximityTarget != null)
            {
                AimAtTarget(currentProximityTarget);    
                return;
            }

            var newTarget = GetFirstAvailableTarget();
            if (newTarget != null && currentProximityTarget?.Targetable.GetInstanceID() != newTarget.Id)
            {
                BroadcastSetTarget(newTarget);
                AimAtTarget(newTarget);
            }
            else if (newTarget != null) 
                AimAtTarget(newTarget);
        }

        void ReleaseTargetFocus()
        {
            if (currentProximityTarget == null)
                return;

            AimAtCamera();
            targetingStrategy.ReleaseTargetFocus();
            SetCurrentProximityTarget(null);
            Targetable.DispatchTargetChanged();
        }

        void UpdateCameraMotor()
        {
            if (cameraController == null || (targetingCameraMotor == null && aimingCameraMotor == null))
                return;
            
            if (targetingCameraMotor != cameraController.currentCameraMotor && aimingCameraMotor != cameraController.currentCameraMotor)
                // Refresh main camera motor as it might have been switched since the Targeter component was created.
                mainCameraMotor = cameraController.currentCameraMotor;

            if (targetingCameraMotor != null && HasTarget())
            {
                var currentProximityTargetPosition = currentProximityTarget.GetCenterOfMassPosition(Vector3.zero);
                var targetPosition = new TargetPosition(TargetPosition.Target.Position) {
                    targetPosition = currentProximityTargetPosition
                };

                var isCameraMotorTypeTarget = targetingCameraMotor.cameraMotorType.GetType() == typeof(CameraMotorTypeTarget);
                var isCameraMotorTypeTargeting = targetingCameraMotor.cameraMotorType.GetType() == typeof(CameraMotorTypeTargeting);
                if (cameraController.currentCameraMotor != targetingCameraMotor)
                {
                    if (isCameraMotorTypeTarget)
                        ((CameraMotorTypeTarget)targetingCameraMotor.cameraMotorType).target = targetPosition;
                    else if (isCameraMotorTypeTargeting)
                        ((CameraMotorTypeTargeting)targetingCameraMotor.cameraMotorType).target = targetPosition;

                    cameraController.ChangeCameraMotor(targetingCameraMotor, startTargetingTransitionDuration);
                    return;
                }

                var refreshCameraMotorTypeTarget = isCameraMotorTypeTarget && ((CameraMotorTypeTarget)targetingCameraMotor.cameraMotorType).target.targetPosition != currentProximityTargetPosition;
                var refreshCameraMotorTypeTargeting = isCameraMotorTypeTargeting && ((CameraMotorTypeTargeting)targetingCameraMotor.cameraMotorType).target.targetPosition != currentProximityTargetPosition;
                if (cameraController.currentCameraMotor == targetingCameraMotor && (refreshCameraMotorTypeTarget || refreshCameraMotorTypeTargeting))
                {
                    if (refreshCameraMotorTypeTarget)
                        ((CameraMotorTypeTarget)targetingCameraMotor.cameraMotorType).target = targetPosition;
                    else if (refreshCameraMotorTypeTargeting)
                        ((CameraMotorTypeTargeting)targetingCameraMotor.cameraMotorType).target = targetPosition;
                    return;
                }
            }
            else if (!HasTarget() && IsAiming())
            {
                // Is aiming without a target.
                if (aimingCameraMotor != null && cameraController.currentCameraMotor != aimingCameraMotor)
                    cameraController.ChangeCameraMotor(aimingCameraMotor, startAimingTransitionDuration);
                else if (aimingCameraMotor == null && cameraController.currentCameraMotor != mainCameraMotor)
                    cameraController.ChangeCameraMotor(mainCameraMotor, startAimingTransitionDuration);
            }
            else if (!HasTarget() && !IsAiming() && cameraController.currentCameraMotor != mainCameraMotor)
                cameraController.ChangeCameraMotor(mainCameraMotor, stopTargetingTransitionDuration);
        }

        void AimAtTarget(ProximityTarget proximityTarget)
        {
            if (aimingAtTarget == null)
                return;

            var isAimingAtTargetAlready = aimingAtTarget.IsAiming && aimingAtTarget.IsAimingAtTarget(proximityTarget);

            if (isAimingAtTargetAlready)
                return;

            if (!isAimingAtTargetAlready)
                aimingAtTarget.SetTarget(characterShooter, proximityTarget);

            if (!isAimingAtTargetAlready || !targetingStrategy.HasTargetFocus())
            {
                // Start Targeting
                targetingStrategy.SetTargetFocus(proximityTarget);
            }

            if (autoAimAtTarget && characterShooter != null && characterShooter.IsAiming)
            {
                // Start Aiming
                characterShooter.StartAiming(aimingAtTarget, false);
            }
        }

        void AimAtCamera()
        {
            aimingAtTarget.UnsetTarget();
            targetingStrategy.ReleaseTargetFocus();
            if (HasCharacterShooter() && characterShooter.IsAiming)
                characterShooter.StartAiming();
        }

        void BroadcastContinuingToBeTargeted(ProximityTarget proximityTarget)
        {
            Targetable.DispatchContinuingToBeTargeted(proximityTarget.GameObject);
        }

        void BroadcastSetTarget(ProximityTarget proximityTarget)
        {
            Targetable.DispatchTargetChanged(proximityTarget.GameObject);
            SetCurrentProximityTarget(proximityTarget);
            broadcastContinuingToBeTargetedTimer = 0f;
        }

        bool CanSeeTarget(ProximityTarget proximityTarget)
        {
            if (!onlyTargetVisibleToCamera || !onlyTargetNonOccluded || !layersToIgnoreForVisibilityOcclusion.Any())
                return true;

            var camera = HookManager.GetCamera();

            // Bit shift the index of the layer to get a bit mask.
            int layerMask = -1;
            foreach (var unityLayer in layersToIgnoreForVisibilityOcclusion)
                layerMask = layerMask == -1 ? unityLayer.Mask : layerMask | unityLayer.Mask;

            // Invert the bit mask so that layers are not collided with.
            layerMask = ~layerMask;

            if (Physics.Linecast(camera.transform.position, proximityTarget.Position, out RaycastHit raycastHit, layerMask))
                return ReferenceEquals(proximityTarget.GameObject, raycastHit.collider.gameObject);

            return true;
        }

        IEnumerable<ProximityTarget> GetAvailableTargets()
        {
            return proximityTargets
                .Where(x => x.Value.DistanceToTarget > 0 && (!onlyTargetVisibleToCamera || x.Value.IsVisible))
                .Select(x => x.Value);
        }

        ProximityTarget GetFirstAvailableTarget(IEnumerable<ProximityTarget> targets = null)
        {
            var availableTargets = (targets ?? GetAvailableTargets()?.OrderBy(x => x.DistanceToTarget)).Where(x => CanSeeTarget(x)).ToList();
            return availableTargets.FirstOrDefault();
        }

        ProximityTarget GetNextTargetByDistance(bool isReverseOrder = false)
        {
            var availableTargets = GetAvailableTargets();

            if (isReverseOrder)
            {
                /**
                 * Get a target that is closer.
                 */
                var descOrderedTargets = availableTargets.OrderByDescending(x => x.DistanceToTarget).ToList();
                return descOrderedTargets.Find(x => x.DistanceToTarget < currentProximityTarget.DistanceToTarget && CanSeeTarget(x))
                    ?? GetFirstAvailableTarget(descOrderedTargets);
            }

            /**
             * Get target that is farther away.
             */
            var ascOrderedTargets = availableTargets.OrderBy(x => x.DistanceToTarget).ToList();
            return ascOrderedTargets.Find(x => x.DistanceToTarget > currentProximityTarget.DistanceToTarget && CanSeeTarget(x))
                ?? GetFirstAvailableTarget(ascOrderedTargets);
        }

        void LateUpdate()
        {
            /**
             * Remove targets that can no longer be targeted. 
             */
            var targetIdsToRemove = proximityTargets.Where(x => !x.Value.CanBeTargeted(TotalTargetingRange)).Select(x => x.Value.Id).ToList();
            foreach (var id in targetIdsToRemove)
                proximityTargets.Remove(id);

            if (characterShooter == null)
                return;

            if (characterShooter.IsAiming && !proximityTargets.Any() && targetIdsToRemove.Any())
            {
                /**
                 *  If all remaining targets were removed, then do not lock onto a specific target.
                 */
                ReleaseTargetFocus();
            }
            else if (characterShooter.IsAiming && !proximityTargets.Any())
                /**
                 *  If there are no more available targets, then do not lock onto a specific target.
                 */
                ReleaseTargetFocus();
            else if (characterShooter.IsAiming && currentProximityTarget != null && targetIdsToRemove.Contains(currentProximityTarget.Id))
                /**
                 * If there are other available targets, and the current target has been removed, then disengage lock on current target.
                 * A new target will be selected in the Update() loop.
                 */
                ReleaseTargetFocus();
        }

        /**
         * Public API
         */
        public bool HasTarget()
        {
            return currentProximityTarget?.GameObject != null;
        }

        public GameObject GetCurrentTarget()
        {
            return currentProximityTarget.GameObject;
        }

        public bool IsCurrentTarget(GameObject target)
        {
            return currentProximityTarget != null && currentProximityTarget.Id == target.GetInstanceID();
        }

        public void SetCurrentTarget(GameObject gameObject)
        {
            if (!isTargetingEnabled || gameObject == null || !proximityTargets.Any())
                return;

            var proximityTarget = proximityTargets.Select(x => x.Value).FirstOrDefault(x => x.GameObject == gameObject);
            if (proximityTarget == null)
                return;

            var previousTargetId = currentProximityTarget == null ? 0 : currentProximityTarget.Id;

            SetCurrentProximityTarget(proximityTarget);

            if (currentProximityTarget.Id != previousTargetId)
                // Switch to new target and indicate that there is a new target.
                BroadcastSetTarget(currentProximityTarget);

            AimAtTarget(currentProximityTarget);
        }

        public void SetTargetingEnabled(bool isEnabled)
        {
            isTargetingEnabled = isEnabled;
        }

        public void ToggleTargetingEnabled()
        {
            SetTargetingEnabled(!isTargetingEnabled);
        }

        public void CycleToNextTarget(bool isReverseSelection = false)
        {
            if (currentProximityTarget == null)
                SetInitialTarget();
            else
            {
                var previousTargetId = currentProximityTarget.Id;
                SetCurrentProximityTarget(GetNextTargetByDistance(isReverseSelection) ?? currentProximityTarget);

                if (characterShooter == null || !characterShooter.IsChargingShot)
                {
                    if (currentProximityTarget.Id != previousTargetId)
                        // Switch to new target and indicate that there is a new target.
                        BroadcastSetTarget(currentProximityTarget);

                    AimAtTarget(currentProximityTarget);
                }
            }
        }

        public void AcquireInitialTarget()
        {
            SetInitialTarget(false);
        }

        public void StartAimingAtTarget()
        {
            if (currentProximityTarget == null || currentProximityTarget.GameObject == null)
                return;
            aimingAtTarget.SetTarget(characterShooter, currentProximityTarget);
            characterShooter?.StartAiming(aimingAtTarget, false);
            AimAtTarget(currentProximityTarget);
        }

        public void StopAimingAtTarget()
        {
            AimAtCamera();
        }

        public bool IsAimingAtTarget()
        {
            return currentProximityTarget != null && aimingAtTarget != null && aimingAtTarget.IsAimingAtTarget(currentProximityTarget);
        }

        public bool IsAiming()
        {
            return HasCharacterShooter() && characterShooter.IsAiming;
        }

        public bool IsProximityTargeter()
        {
            return targetingType == TargetingType.Proximity;
        }

        public bool IsAimAssistTargeter()
        {
            return targetingType == TargetingType.AimAssist;
        }

        public bool IsMouseTargeter()
        {
            return targetingType == TargetingType.Mouse;
        }

        public bool HasCharacterShooter()
        {
            return characterShooter != null;
        }
    }
}
