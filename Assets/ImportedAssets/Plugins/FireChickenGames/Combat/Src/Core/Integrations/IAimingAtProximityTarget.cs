namespace FireChickenGames.Combat.Core.Integrations
{
    using FireChickenGames.Combat.Core.Targeting;

    public interface IAimingAtProximityTarget
    {
        void UnsetTarget();
        bool IsAiming { get; }
        bool IsAimingAtTarget(ProximityTarget proximityTarget);
        void SetTarget(ICharacterShooter characterShooter, ProximityTarget proximityTarget);
        object GetAiming();
    }
}
