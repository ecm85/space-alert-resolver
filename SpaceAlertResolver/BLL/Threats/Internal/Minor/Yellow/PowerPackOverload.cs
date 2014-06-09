using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class PowerPackOverload : MinorYellowInternalThreat
	{
		private ISet<StationLocation> StationsHitThisTurn { get; set; }

		public PowerPackOverload()
			: base(
				4,
				3,
				new List<StationLocation> {StationLocation.LowerBlue, StationLocation.LowerRed},
				PlayerAction.A)
		{
			StationsHitThisTurn = new HashSet<StationLocation>();
		}

		public static string GetDisplayName()
		{
			return "Power Pack Overload";
		}

		public override void PerformXAction()
		{
			SittingDuck.DisableInactiveBattlebots(new[] {StationLocation.LowerRed});
			SittingDuck.RemoveRocket();
		}

		public override void PerformYAction()
		{
			Repair(1);
		}

		public override void PerformZAction()
		{
			SittingDuck.KnockOutPlayers(CurrentStations);
			Damage(3, CurrentZones);
		}

		protected override void PerformEndOfPlayerActionsOnTrack()
		{
			if (CurrentStations.All(station => StationsHitThisTurn.Contains(station)))
				base.TakeDamageOnTrack(1, null, false, CurrentStation);
			StationsHitThisTurn.Clear();
			base.PerformEndOfPlayerActionsOnTrack();
		}

		protected override void TakeDamageOnTrack(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			StationsHitThisTurn.Add(stationLocation);
			base.TakeDamageOnTrack(damage, performingPlayer, isHeroic, stationLocation);
		}
	}
}
