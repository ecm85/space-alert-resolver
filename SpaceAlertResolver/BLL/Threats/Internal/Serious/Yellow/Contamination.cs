using System.Collections.Generic;
using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Yellow
{
    public class Contamination : SeriousYellowInternalThreat
    {
        internal Contamination()
            : base(
                3,
                2,
                new List<StationLocation>
                {
                    StationLocation.UpperBlue,
                    StationLocation.UpperRed,
                    StationLocation.LowerBlue,
                    StationLocation.LowerRed
                },
                PlayerActionType.BattleBots)
        {
        }
        protected override void PerformXAction(int currentTurn)
        {
            SittingDuck.ShiftPlayersAfterPlayerActions(CurrentStations, currentTurn);
        }

        protected override void PerformYAction(int currentTurn)
        {
            AttackSpecificZones(1, CurrentZones);
        }

        protected override void PerformZAction(int currentTurn)
        {
            SittingDuck.KnockOutPlayers(CurrentStations);
            SittingDuck.SubscribeToMovingIn(CurrentStations, KnockOutPlayer);
        }

        public override string Id { get; } = "SI2-04";
        public override string DisplayName { get; } = "Contamination";
        public override string FileName { get; } = "Contamination";

        private static void KnockOutPlayer(object sender, PlayerMoveEventArgs args)
        {
            args.MovingPlayer.KnockOut();
        }

        public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
        {
            base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
            if (stationLocation != null)
                CurrentStations.Remove(stationLocation.Value);
        }
    }
}
