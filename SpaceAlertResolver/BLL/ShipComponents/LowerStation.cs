using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class LowerStation : StandardStation<Reactor>
	{
		public LowerStation(
			StationLocation stationLocation,
			ThreatController threatController,
			ICharlieComponent charlieComponent,
			Gravolift gravolift,
			Airlock bluewardAirlock,
			Airlock redwardAirlock,
			Cannon cannon,
			SittingDuck sittingDuck,
			Reactor reactor) : base(stationLocation, threatController, reactor, charlieComponent, gravolift, bluewardAirlock, redwardAirlock, cannon, sittingDuck)
		{
		}

		public override void DrainEnergyContainer(int amount)
		{
			BravoComponent.Energy -= amount;
		}
	}
}
