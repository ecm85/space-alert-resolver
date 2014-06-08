using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class PowerPackOverload : MinorYellowInternalThreat
	{
		private ISet<StationLocation> StationsHitThisTurn { get; set; }

		public PowerPackOverload(int timeAppears, ISittingDuck sittingDuck)
			: base(
				4,
				3,
				timeAppears,
				new List<StationLocation> {StationLocation.LowerBlue, StationLocation.LowerRed},
				PlayerAction.A,
				sittingDuck)
		{
			StationsHitThisTurn = new HashSet<StationLocation>();
		}

		public static string GetDisplayName()
		{
			return "Power Pack Overload";
		}

		public override void PeformXAction()
		{
			sittingDuck.DisableInactiveBattlebots(new[] {StationLocation.LowerRed});
			sittingDuck.RemoveRocket();
		}

		public override void PerformYAction()
		{
			Repair(1);
		}

		public override void PerformZAction()
		{
			sittingDuck.KnockOutPlayers(CurrentStations);
			Damage(3, CurrentZones);
		}

		public override void PerformEndOfPlayerActions()
		{
			if (CurrentStations.All(station => StationsHitThisTurn.Contains(station)))
				base.TakeDamage(1, null, false, CurrentStation);
			StationsHitThisTurn.Clear();
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			StationsHitThisTurn.Add(stationLocation);
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
		}
	}
}
