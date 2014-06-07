using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public class CentralLaserJam : MinorWhiteInternalThreat
	{
		public CentralLaserJam(int timeAppears, ISittingDuck sittingDuck)
			: base(2, 2, timeAppears, StationLocation.UpperWhite, PlayerAction.A, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			sittingDuck.StationByLocation[CurrentStation].OppositeDeckStation.EnergyContainer.Energy -= 1;
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

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			//TODO: This is incorrect. if remaining health is 2 and damage is 2, need to still drain reactors
			if (RemainingHealth == 1)
			{
				var reactor = sittingDuck.StationByLocation[CurrentStation].OppositeDeckStation.EnergyContainer;
				if (reactor.Energy > 1)
				{
					reactor.Energy--;
					base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
				}
			}
			else
				base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
		}
	}
}
