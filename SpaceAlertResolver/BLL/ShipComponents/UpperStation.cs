using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class UpperStation : StandardStation
	{
		public Shield Shield { get; private set; }

		public UpperStation(
			StationLocation stationLocation,
			ThreatController threatController,
			ICharlieComponent charlieComponent,
			Gravolift gravolift,
			Airlock bluewardAirlock,
			Airlock redwardAirlock,
			Cannon cannon,
			SittingDuck sittingDuck,
			Shield shield) : base(stationLocation, threatController, charlieComponent, gravolift, bluewardAirlock, redwardAirlock, cannon, sittingDuck)
		{
			Shield = shield;
		}

		protected override void RefillEnergy(bool isHeroic)
		{
			Shield.PerformBAction(isHeroic);
		}

		public override void DrainEnergyContainer(int amount)
		{
			Shield.Energy -= amount;
		}

		public override void PerformEndOfTurn()
		{
			base.PerformEndOfTurn();
			Shield.PerformEndOfTurn();
		}
	}
}
