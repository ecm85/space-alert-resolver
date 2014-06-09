using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public class CentralLaserJam : MinorWhiteInternalThreat
	{
		public CentralLaserJam()
			: base(2, 2, StationLocation.UpperWhite, PlayerAction.A)
		{
		}

		public override void PerformXAction()
		{
			SittingDuck.DrainReactors(CurrentZones, 1);
		}

		public override void PerformYAction()
		{
			Damage(1);
		}

		public override void PerformZAction()
		{
			Damage(3);
			DamageOtherTwoZones(1);
		}

		public static string GetDisplayName()
		{
			return "Central Laser Jam";
		}

		protected override void TakeDamageOnTrack(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			var remainingDamageWillDestroyThreat = RemainingHealth <= damage;
			var energyDrained = 0;
			if (remainingDamageWillDestroyThreat)
				energyDrained = SittingDuck.DrainReactors(CurrentZones, 1);
			var cannotTakeDamage = remainingDamageWillDestroyThreat && energyDrained == 0;
			if (!cannotTakeDamage)
				base.TakeDamageOnTrack(damage, performingPlayer, isHeroic, stationLocation);
		}
	}
}
