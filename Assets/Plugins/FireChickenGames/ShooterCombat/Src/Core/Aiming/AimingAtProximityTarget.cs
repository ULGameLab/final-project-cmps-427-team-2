namespace FireChickenGames.ShooterCombat.Core.Aiming
{
    using FireChickenGames.Combat.Core.Integrations;
    using FireChickenGames.Combat.Core.Targeting;
    using GameCreator.Characters;
    using GameCreator.Core;
    using GameCreator.Shooter;
    using UnityEngine;

    public class AimingAtProximityTarget : AimingAtTarget, IAimingAtProximityTarget
    {
        public ProximityTarget ProximityTarget { get; internal set; }
        public bool IsAiming { get { return shooter != null && shooter.isAiming; } }

        public AimingAtProximityTarget(ICharacterShooter characterShooter)
            : base(characterShooter.GetCharacterShooter() as CharacterShooter)
        {
        }

        public void SetTarget(ICharacterShooter characterShooter, ProximityTarget proximityTarget)
        {
            if (shooter == null || character == null)
                InitializeCharacterShooter(characterShooter);

            if (proximityTarget == null && proximityTarget.GameObject == null)
                target = null;
            else if (target == null || target.gameObject == null || proximityTarget.Id != target.gameObject.GetInstanceID())
            {
                target = new TargetGameObject(TargetGameObject.Target.GameObject) { gameObject = proximityTarget.GameObject };
                ProximityTarget = proximityTarget;
            }
        }

        void InitializeCharacterShooter(ICharacterShooter characterShooter)
        {
            shooter = characterShooter.GetCharacterShooter() as CharacterShooter;

            if (shooter.character != null)
            {
                character = shooter.character;
                shootingAnchor = character.GetCharacterAnimator()
                    .animator
                    .GetBoneTransform(HumanBodyBones.Chest);
            }
            else
            {
                turret = shooter.gameObject;
                shootingAnchor = turret.transform;
            }
        }

        public void UnsetTarget()
        {
            ProximityTarget = null;
            target = null;
        }

        public override void Update()
        {
            if (target == null)
                return;
            base.Update();
        }

        public bool IsAimingAtTarget(ProximityTarget proximityTarget)
        {
            if (ProximityTarget == null || ProximityTarget.GameObject == null || proximityTarget == null)
                // There cannot possibly be a target if all the targets are null.
                return false;
            return proximityTarget.Id == ProximityTarget.Id;
        }

        protected override void UpdateAimToVectorFromTarget()
        {
            /**
             * Fallback to the forward direction of the shooter.
             */
            aimToVector = ProximityTarget.GetCenterOfMassPosition(shootingAnchor.position + shootingAnchor.TransformDirection(Vector3.forward));
        }

        public object GetAiming()
        {
            return shooter.aiming;
        }
    }
}
