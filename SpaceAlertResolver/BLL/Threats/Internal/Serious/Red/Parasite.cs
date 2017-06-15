using System.Collections.Generic;
using System.Linq;
using BLL.Common;
using BLL.Players;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Serious.Red
{
    public class Parasite : SeriousRedInternalThreat
    {
        private Player attachedPlayer;

        internal Parasite()
            : base(1, 2, new List<StationLocation>(), PlayerActionType.BattleBots)
        {
        }

        public override void PlaceOnTrack(Track track, int trackPosition)
        {
            base.PlaceOnTrack(track, trackPosition);
            SittingDuck.SubscribeToMovingIn(EnumFactory.All<StationLocation>().Where(station => station.IsOnShip()), AttachToPlayer);
        }

        private void AttachToPlayer(object sender, PlayerMoveEventArgs args)
        {
            attachedPlayer = args.MovingPlayer;
            SittingDuck.UnsubscribeFromMovingIn(EnumFactory.All<StationLocation>().Where(station => station.IsOnShip()), AttachToPlayer);
        }

        protected override void PerformXAction(int currentTurn)
        {
            if (attachedPlayer != null && !attachedPlayer.IsKnockedOut && attachedPlayer.CurrentStation.StationLocation.IsOnShip())
                SittingDuck.DrainEnergy(attachedPlayer.CurrentStation.StationLocation, 1);
        }

        protected override void PerformYAction(int currentTurn)
        {
            var otherPlayersInStation = SittingDuck.GetPlayersInStation(attachedPlayer.CurrentStation.StationLocation)
                .Except(new [] {attachedPlayer});
            foreach (var player in otherPlayersInStation)
                player.KnockOut();
        }

        protected override void PerformZAction(int currentTurn)
        {
            if (attachedPlayer != null)
            {
                if (attachedPlayer.Interceptors != null)
                    AttackSpecificZone(5, ZoneLocation.White, ThreatDamageType.Standard);
                else
                    Attack(5);
            }
        }

        public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
        {
            Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
            base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
            if(IsDefeated)
                attachedPlayer.KnockOut();
            if (!isHeroic)
                performingPlayer.BattleBots.IsDisabled = true;
        }

        public override bool CanBeTargetedBy(StationLocation stationLocation, PlayerActionType playerActionType, Player performingPlayer)
        {
            Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
            return ActionType == playerActionType &&
                CurrentStations.Contains(performingPlayer.CurrentStation.StationLocation)
                && performingPlayer != attachedPlayer;
        }

        protected override void OnThreatTerminated()
        {
            base.OnThreatTerminated();
            SittingDuck.UnsubscribeFromMovingIn(EnumFactory.All<StationLocation>().Where(station => station.IsOnShip()), AttachToPlayer);
        }

        public override string Id { get; } = "SI3-107";
        public override string DisplayName { get; } = "Parasite";
        public override string FileName { get; } = "Parasite";
    }
}
