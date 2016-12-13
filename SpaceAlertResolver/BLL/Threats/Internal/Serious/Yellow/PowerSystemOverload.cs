using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class PowerSystemOverload : SeriousYellowInternalThreat
	{
		private ISet<StationLocation> StationsHitThisTurn { get; set; }

		public PowerSystemOverload()
			: base(
				7,
				3,
				new List<StationLocation> {StationLocation.LowerRed, StationLocation.LowerWhite, StationLocation.LowerBlue},
				PlayerActionType.B)
		{
			StationsHitThisTurn = new HashSet<StationLocation>();
		}

		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			ThreatController.PlayerActionsEnding += OnPlayerActionsEnding;
		}
		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.DrainReactor(ZoneLocation.White, 2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.DrainReactors(EnumFactory.All<ZoneLocation>(), 1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			DamageAllZones(3);
		}

		private void OnPlayerActionsEnding(object sender, EventArgs args)
		{
			if (CurrentStations.All(station => StationsHitThisTurn.Contains(station)))
				base.TakeDamage(2, null, false, CurrentStation);
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
			ThreatController.PlayerActionsEnding -= OnPlayerActionsEnding;
			base.OnThreatTerminated();
		}
	}
}
