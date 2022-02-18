namespace FireChickenGames.Combat
{
    using GameCreator.Core;
    using UnityEngine;

    [AddComponentMenu("Fire Chicken Games/Combat/Homing Projectile")]
    public class HomingProjectile : MonoBehaviour
    {
        public enum TargetingMode
        {
            Target = 0,
            Targeter = 1,
        }

        #region Editor Properties
        public TargetingMode targetMode = TargetingMode.Targeter;

        [Tooltip("Any game object with transform that will be targeted.")]
        public TargetGameObject targetGameObject = new TargetGameObject(TargetGameObject.Target.GameObject);

        [Tooltip("A game object with a Targeter component in its hierarchy.")]
        public TargetGameObject targeterGameObject = new TargetGameObject(TargetGameObject.Target.Player);

        [Tooltip("The Rigidbody component to propel - this is automatically set if the game object also has a Rigidbody component attached.")]
        public Rigidbody ammoRigidbody;

        [Header("Propulsion")]
        [Tooltip("An offset to add to the target's transform.")]
        public Vector3 targetOffset = Vector3.zero;
        [Tooltip("Delays the propulsion of the projectile by a number of seconds. A value of 0 (or less) results in no delay.")]
        public float secondsToWaitBeforePropelling = 0.5f;
        [Tooltip("Stop propelling the projectile after a number of seconds - should be greater than Propulsion Delay.")]
        public float propelForPeriodInSeconds = 10f;
        [Tooltip("The maximum angle, in degrees, that the projectile will turn while homing in on its target.")]
        public float maximumTurnAngle = 10.0f;
        [Tooltip("How fast the projectile moves toward its target.")]
        public float velocity = 25.0f;
        #endregion

        #region Public API Fields
        public bool IsTargetingModeTargeter => targetMode == TargetingMode.Targeter;
        public bool IsTargetingModeTarget => targetMode == TargetingMode.Target;

        [HideInInspector]
        public Transform targetTransform;
        [HideInInspector]
        public Targeter targeter;
        #endregion

        #region Private Fields
        private float propulsionTimer = 0f;
        private const float VELOCITY_MULTIPLIER = 100.0f;
        #endregion

        void Start()
        {
            OnEnable();
        }

        void OnEnable()
        {
            /**
             * The internal propulsion timer must be reset if the projectile prefab is a pool object.
             */
            propulsionTimer = 0f;

            if (ammoRigidbody == null && !gameObject.TryGetComponent(out ammoRigidbody))
                ammoRigidbody = gameObject.AddComponent<Rigidbody>();

            targetTransform = GetTargetTransform();
        }

        Transform GetTargetTransform()
        {
            if (targetMode == TargetingMode.Target)
            {
                var targetGO = targetGameObject.GetGameObject(gameObject);
                if (targetGO != null)
                    return targetGO.transform;
            }
            else if (targetMode == TargetingMode.Targeter)
            {
                if (targeter == null)
                {
                    var targeterGO = targeterGameObject.GetGameObject(gameObject);
                    if (targeterGO != null)
                    {
                        targeter = targeterGO.GetComponent<Targeter>();
                        if (targeter == null)
                            targeter = targeterGO.GetComponentInChildren<Targeter>();
                    }
                }

                if (targeter != null && targeter.currentProximityTarget != null)
                    return targeter.currentProximityTarget.Transform;
            }
            return targetTransform;
        }

        void FixedUpdate()
        {
            targetTransform = GetTargetTransform();

            if (targetTransform == null)
                return;

            // Translation.
            propulsionTimer += Time.deltaTime;
            if (propulsionTimer < secondsToWaitBeforePropelling)
                return;

            if (propulsionTimer > propelForPeriodInSeconds)
                return;

            ammoRigidbody.velocity = transform.forward * velocity * VELOCITY_MULTIPLIER * Time.deltaTime;

            // Rotation.
            var targetRotation = Quaternion.LookRotation((targetTransform.position + targetOffset) - transform.position);
            ammoRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, maximumTurnAngle));
        }
    }
}
