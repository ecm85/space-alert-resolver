using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
				PlayerAction.B)
		{
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			ThreatController.EndOfPlayerActions += PerformEndOfPlayerActions;
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
			ThreatController.EndOfPlayerActions -= PerformEndOfPlayerActions;
		}

		private void PerformEndOfPlayerActions()
		{
			if (attackingPlayersThisTurn.Count == 1)
				base.TakeDamage(1, null, false, null);
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			attackingPlayersThisTurn.Add(performingPlayer);
		}

		public static string GetDisplayName()
		{
			return "Reversed Shields";
		}
	}
}
