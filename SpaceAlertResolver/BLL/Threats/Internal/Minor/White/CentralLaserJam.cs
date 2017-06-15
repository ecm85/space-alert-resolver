using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
    public class CentralLaserJam : MinorWhiteInternalThreat
    {
        internal CentralLaserJam()
            : base(2, 2, StationLocation.UpperWhite, PlayerActionType.Alpha)
        {
        }

        protected override void PerformXAction(int currentTurn)
        {
            SittingDuck.DrainReactors(new [] {CurrentZone}, 1);
        }

        protected override void PerformYAction(int currentTurn)
        {
            Attack(1);
        }

        protected override void PerformZAction(int currentTurn)
        {
            Attack(3);
            AttackOtherTwoZones(1);
        }

        public override string Id { get; } = "I1-101";
        public override string DisplayName { get; } = "Central Laser Jam";
        public override string FileName { get; } = "CentralLaserJam";

        public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
        {
            var remainingDamageWillDestroyThreat = RemainingHealth <= damage;
            var energyDrained = 0;
            if (remainingDamageWillDestroyThreat)
                energyDrained = SittingDuck.DrainReactors(new [] {CurrentZone}, 1);
            var cannotTakeDamage = remainingDamageWillDestroyThreat && energyDrained == 0;
            if (!cannotTakeDamage)
                base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
        }
    }
}
