namespace FireChickenGames.Combat.Core.Aiming
{
    using FireChickenGames.Combat.Core.Integrations;
    using FireChickenGames.Combat.Core.Targeting;

    public class FocusOnTarget : IAimingAtProximityTarget
    {
        ProximityTarget currentTarget;

        public bool IsAiming { get { return false; } }

        public object GetAiming()
        {
            return null;
        }

        public bool IsAimingAtTarget(ProximityTarget proximityTarget)
        {
            return currentTarget == proximityTarget;
        }

        public void SetTarget(ICharacterShooter characterShooter, ProximityTarget proximityTarget)
        {
            currentTarget = proximityTarget;
        }

        public void UnsetTarget()
        {
            currentTarget = null;
        }
    }
}
