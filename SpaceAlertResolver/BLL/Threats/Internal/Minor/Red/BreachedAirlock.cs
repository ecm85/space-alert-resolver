using BLL.Common;
using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Red
{
    public class BreachedAirlock : MinorRedInternalThreat
    {
        internal BreachedAirlock()
            : base(3, 4, StationLocation.UpperRed, PlayerActionType.Charlie)
        {
        }

        protected override void PerformXAction(int currentTurn)
        {
            SittingDuck.SealRedDoors();
        }

        protected override void PerformYAction(int currentTurn)
        {
            SittingDuck.SealBlueDoors();
        }

        protected override void PerformZAction(int currentTurn)
        {
            Attack(3);
            SittingDuck.KnockOutPlayers(new [] {CurrentZone});
        }

        protected override void OnHealthReducedToZero()
        {
            base.OnHealthReducedToZero();
            SittingDuck.RepairAllSealedDoors();
        }

        public override string Id { get; } = "I3-104";
        public override string DisplayName { get; } = "Breached Airlock";
        public override string FileName { get; } = "BreachedAirlock";

        public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
        {
            Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
            var bonusDamage = performingPlayer.BattleBots != null && !performingPlayer.BattleBots.IsDisabled ? 1 : 0;
            base.TakeDamage(damage + bonusDamage, performingPlayer, isHeroic, stationLocation);
        }
    }
}
