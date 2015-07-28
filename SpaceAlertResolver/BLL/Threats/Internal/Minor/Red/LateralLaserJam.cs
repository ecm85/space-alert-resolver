using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Minor.Red
{
	public class LateralLaserJam : MinorRedInternalThreat
	{
		public LateralLaserJam()
			: base(3, 3, new List<StationLocation>(), PlayerActionType.A)
		{
		}

		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			CurrentStation = TimeAppears % 2 == 0 ? StationLocation.UpperBlue : StationLocation.UpperRed;
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.DrainReactor(CurrentZone, 2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Damage(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			var otherZone = CurrentZone == ZoneLocation.Blue ? ZoneLocation.Red : ZoneLocation.Blue;
			Damage(3);
			Damage(2, new[] {ZoneLocation.White});
			Damage(1, new [] {otherZone});
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			var remainingDamageWillDestroyThreat = RemainingHealth <= damage;
			var energyDrained = 0;
			if (remainingDamageWillDestroyThreat)
				energyDrained = SittingDuck.DrainReactor(CurrentZone, 1);
			var cannotTakeDamage = remainingDamageWillDestroyThreat && energyDrained == 0;
			if (!cannotTakeDamage)
				base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
		}
	}
}
