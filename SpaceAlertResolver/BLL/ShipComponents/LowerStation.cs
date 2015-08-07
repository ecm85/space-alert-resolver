using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class LowerStation : StandardStation
	{
		public Reactor Reactor { get; private set; }

		public LowerStation(
			StationLocation stationLocation,
			ThreatController threatController,
			ICharlieComponent charlieComponent,
			Gravolift gravolift,
			Airlock bluewardAirlock,
			Airlock redwardAirlock,
			Cannon cannon,
			SittingDuck sittingDuck,
			Reactor reactor) : base(stationLocation, threatController, charlieComponent, gravolift, bluewardAirlock, redwardAirlock, cannon, sittingDuck)
		{
			Reactor = reactor;
		}

		protected override void RefillEnergy(bool isHeroic)
		{
			Reactor.PerformBAction(isHeroic);
		}

		public override void DrainEnergyContainer(int amount)
		{
			Reactor.Energy -= amount;
		}
	}
}
