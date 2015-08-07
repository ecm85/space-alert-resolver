using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class UpperStation : StandardStation<Shield>
	{
		public UpperStation(
			StationLocation stationLocation,
			ThreatController threatController,
			ICharlieComponent charlieComponent,
			Gravolift gravolift,
			Airlock bluewardAirlock,
			Airlock redwardAirlock,
			Cannon cannon,
			SittingDuck sittingDuck,
			Shield shield) : base(stationLocation, threatController, shield, charlieComponent, gravolift, bluewardAirlock, redwardAirlock, cannon, sittingDuck)
		{
		}

		public override void DrainEnergyContainer(int amount)
		{
			BravoComponent.Energy -= amount;
		}

		public override void PerformEndOfTurn()
		{
			base.PerformEndOfTurn();
			BravoComponent.PerformEndOfTurn();
		}
	}
}
