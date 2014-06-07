using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class PowerSystemOverload : SeriousYellowInternalThreat
	{
		private ISet<StationLocation> StationsHitThisTurn { get; set; }

		public PowerSystemOverload(int timeAppears, ISittingDuck sittingDuck)
			: base(
				7,
				3,
				timeAppears,
				new List<StationLocation> {StationLocation.LowerRed, StationLocation.LowerWhite, StationLocation.LowerBlue},
				PlayerAction.B,
				sittingDuck)
		{
			StationsHitThisTurn = new HashSet<StationLocation>();
		}

		public static string GetDisplayName()
		{
			return "Power System Overload";
		}

		public override void PeformXAction()
		{
			sittingDuck.DrainReactors(new[] { ZoneLocation.White }, 2);
		}

		public override void PerformYAction()
		{
			sittingDuck.DrainReactors(EnumFactory.All<ZoneLocation>(), 1);
		}

		public override void PerformZAction()
		{
			DamageAllZones(3);
		}

		public override void PerformEndOfPlayerActions()
		{
			if (CurrentStations.All(station => StationsHitThisTurn.Contains(station)))
				base.TakeDamage(2, null, false, CurrentStation);
			StationsHitThisTurn.Clear();
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			StationsHitThisTurn.Add(stationLocation);
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
		}
	}
}
