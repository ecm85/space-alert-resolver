using System;
using System.Collections.Generic;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Minor.Red
{
	public class ReversedShields : MinorRedInternalThreat
	{
		private readonly ISet<Player> attackingPlayersThisTurn = new HashSet<Player>();
		public ReversedShields()
			: base(
				5,
				4,
				new List<StationLocation> {StationLocation.UpperBlue, StationLocation.UpperWhite, StationLocation.UpperRed},
				PlayerActionType.Bravo)
		{
		}

		public override void PlaceOnBoard(Track track, int trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			ThreatController.PlayerActionsEnding += OnPlayerActionsEnding;
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.AddZoneDebuff(EnumFactory.All<ZoneLocation>(), ZoneDebuff.IneffectiveShields, this);
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.RemoveZoneDebuffForSource(EnumFactory.All<ZoneLocation>(), this);
			SittingDuck.AddZoneDebuff(EnumFactory.All<ZoneLocation>(), ZoneDebuff.ReversedShields, this);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(2);
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnHealthReducedToZero();
			SittingDuck.RemoveZoneDebuffForSource(EnumFactory.All<ZoneLocation>(), this);
		}

		protected override void OnThreatTerminated()
		{
			base.OnThreatTerminated();
			ThreatController.PlayerActionsEnding -= OnPlayerActionsEnding;
		}

		public override string Id { get; } = "I3-103";
		public override string DisplayName { get; } = "Reversed Shields";
		public override string FileName { get; } = "ReversedShields";

		private void OnPlayerActionsEnding(object sender, EventArgs args)
		{
			if (attackingPlayersThisTurn.Count == 1)
				TakeDamage(1, null, false, null);
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (performingPlayer != null)
				attackingPlayersThisTurn.Add(performingPlayer);
		}
	}
}
