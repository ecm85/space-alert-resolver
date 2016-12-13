namespace BLL.ShipComponents
{
	public class LowerStation : StandardStation
	{
		private Reactor Reactor { get; }

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
			Reactor = reactor;
		}

		public override void DrainEnergy(int amount)
		{
			DrainReactor(amount);
		}

		public int DrainReactor()
		{
			var oldEnergy = Reactor.Energy;
			Reactor.Energy = 0;
			var currentEnergy = Reactor.Energy;
			return oldEnergy - currentEnergy;
		}

		public int DrainReactor(int amount)
		{
			var oldEnergy = Reactor.Energy;
			Reactor.Energy -= amount;
			var currentEnergy = Reactor.Energy;
			return oldEnergy - currentEnergy;
		}

		public int EnergyInReactor => Reactor.Energy;
	}
}
