using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Serious.White
{
	public class BattleBotUprising : SeriousWhiteInternalThreat
	{
		private ISet<StationLocation> StationsHitThisTurn { get; set; }

		public BattleBotUprising()
			: base(4, 2,new List<StationLocation> {StationLocation.UpperBlue, StationLocation.LowerRed}, PlayerActionType.Charlie)
		{
			StationsHitThisTurn = new HashSet<StationLocation>();
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			ThreatController.PlayerActionsEnding += OnPlayerActionsEnding;
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayersWithBattleBots(EnumFactory.All<StationLocation>().Where(stationLocation => stationLocation.IsOnShip()));
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayers(CurrentStations);
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayers(EnumFactory.All<StationLocation>().Where(stationLocation => stationLocation.IsOnShip()).Except(new[] { StationLocation.UpperWhite }));
		}
		private void OnPlayerActionsEnding(object sender, EventArgs args)
		{
			if (CurrentStations.All(station => StationsHitThisTurn.Contains(station)))
				base.TakeDamage(1, null, false, CurrentStation);
			StationsHitThisTurn.Clear();
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			if (stationLocation != null)
				StationsHitThisTurn.Add(stationLocation.Value);
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
		}

		protected override void OnThreatTerminated()
		{
			base.OnThreatTerminated();
			ThreatController.PlayerActionsEnding -= OnPlayerActionsEnding;
		}

		public override string Id { get; } = "SI1-06";
		public override string DisplayName { get; } = "BattleBot Uprising";
		public override string FileName { get; } = "BattleBotUprising";
	}
}
