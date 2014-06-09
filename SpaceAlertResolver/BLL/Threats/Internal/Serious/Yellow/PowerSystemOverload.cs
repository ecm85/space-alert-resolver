using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
				PlayerAction.B)
		{
			StationsHitThisTurn = new HashSet<StationLocation>();
		}

		public static string GetDisplayName()
		{
			return "Power System Overload";
		}

		public override void PerformXAction()
		{
			SittingDuck.DrainReactors(new[] { ZoneLocation.White }, 2);
		}

		public override void PerformYAction()
		{
			SittingDuck.DrainReactors(EnumFactory.All<ZoneLocation>(), 1);
		}

		public override void PerformZAction()
		{
			DamageAllZones(3);
		}

		protected override void PerformEndOfPlayerActionsOnTrack()
		{
			if (CurrentStations.All(station => StationsHitThisTurn.Contains(station)))
				base.TakeDamageOnTrack(2, null, false, CurrentStation);
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
