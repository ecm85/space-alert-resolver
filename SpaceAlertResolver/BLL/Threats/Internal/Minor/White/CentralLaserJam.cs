using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
	public class CentralLaserJam : MinorWhiteInternalThreat
	{
		public CentralLaserJam()
			: base(2, 2, StationLocation.UpperWhite, PlayerActionType.A)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.DrainReactor(CurrentZone, 1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Damage(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(3);
			DamageOtherTwoZones(1);
		}

		public static string GetDisplayName()
		{
			return "Central Laser Jam";
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

		public static string GetId()
		{
			return "I1-101";
		}
	}
}
