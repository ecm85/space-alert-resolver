namespace BLL.ShipComponents
{
	public abstract class LowerStation : StandardStation
	{
		public abstract Reactor Reactor { get; }

		public override IBravoComponent BravoComponent => Reactor;

		protected LowerStation(
			StationLocation stationLocation,
			ThreatController threatController,
			Gravolift gravolift,
			Airlock bluewardAirlock,
			Airlock redwardAirlock,
			SittingDuck sittingDuck) : base(stationLocation, threatController, gravolift, bluewardAirlock, redwardAirlock, sittingDuck)
		{
		}

		public override void DrainEnergy(int amount)
		{
			DrainReactor(amount);
		}

		public int EmptyReactor()
		{
			return Reactor.Empty();
		}

		public int DrainReactor(int amount)
		{
			return Reactor.Drain(amount);
		}

		public int EnergyInReactor => Reactor.Energy;
	}
}
