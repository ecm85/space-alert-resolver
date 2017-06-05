using System.Collections.Generic;
using BLL.Players;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Minor.Red
{
	public class LateralLaserJam : MinorRedInternalThreat
	{
		internal LateralLaserJam()
			: base(3, 3, new List<StationLocation>(), PlayerActionType.Alpha)
		{
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			CurrentStation = TimeAppears % 2 == 0 ? StationLocation.UpperBlue : StationLocation.UpperRed;
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.DrainReactors(new [] {CurrentZone}, 2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			var otherZone = CurrentZone == ZoneLocation.Blue ? ZoneLocation.Red : ZoneLocation.Blue;
			Attack(3);
			AttackSpecificZone(2, ZoneLocation.White);
			AttackSpecificZone(1, otherZone);
		}

		public override string Id { get; } = "I3-101";
		public override string DisplayName { get; } = "Lateral Laser Jam";
		public override string FileName { get; } = "LateralLaserJam";

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			var remainingDamageWillDestroyThreat = RemainingHealth <= damage;
			var energyDrained = 0;
			if (remainingDamageWillDestroyThreat)
				energyDrained = SittingDuck.DrainReactors(new [] {CurrentZone}, 1);
			var cannotTakeDamage = remainingDamageWillDestroyThreat && energyDrained == 0;
			if (!cannotTakeDamage)
				base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
		}
	}
}
