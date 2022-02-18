namespace FireChickenGames.Combat.Core.Targeting
{
    using GameCreator.Characters;
    using UnityEngine;

    /// <summary>
    /// This class should never need to be directly instantiated outside of its usage in the Targeter class.
    /// </summary>
    public class ProximityTarget
    {
        public int Id { get; private set; }
        public Vector3 Position {
            get {
                return Targetable == null || Targetable.gameObject == null || Targetable.gameObject.transform == null ?
                    new Vector3() :
                    Targetable.gameObject.transform.position;
            }
        }
        public float DistanceToTarget { get; set; }

        private Character character;
        private CharacterController characterController;
        private Targetable _targetable;
        public Targetable Targetable
        {
            get { return _targetable; }
            set
            {
                _targetable = value;
                Id = _targetable.gameObject.gameObject.GetInstanceID();
                _targetable.gameObject.TryGetComponent(out character);
                _targetable.gameObject.TryGetComponent(out characterController);
            }
        }

        public GameObject GameObject { get { return Targetable == null ? null : Targetable.gameObject; } }
        public Transform Transform { get { return Targetable == null ? null : Targetable.transform; } }
        public bool IsVisible { get { return Targetable == null ? false : Targetable.IsVisible(); } }
        public bool IsHumanoid { get {
                return character != null && character.GetCharacterAnimator().animator.isHuman;
            }
        }
        public bool IsGenericHumanoid { get { return character == null && characterController != null; } }

        public ProximityTarget() {}

        public ProximityTarget(Targetable targetable) : this()
        {
            Targetable = targetable;
        }

        public bool CanBeTargeted(float targetingRange)
        {
            return Targetable != null && Targetable.CanBeTargeted() && DistanceToTarget < targetingRange;
        }

        public Vector3 GetCenterOfMassPosition(Vector3 defaultPosition)
        {
            if (IsHumanoid)
            {
                var characterTransform = character
                    .GetCharacterAnimator()
                    .animator
                    .GetBoneTransform(HumanBodyBones.Chest);
                if (characterTransform != null)
                    return characterTransform.position;
            }

            /**
             * Aim at a non-humanoid target.
             */
            if (Transform != null)
            {
                var centerOfMass = Transform.position;
                if (IsGenericHumanoid)
                    // A non-humanoid target might still have a character controller if it is a character with a generic model.
                    centerOfMass.y += (characterController.height / 3f) * 2f;
                return centerOfMass;
            }

            return defaultPosition;
        }
    }
}
