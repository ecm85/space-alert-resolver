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
			Doors bluewardDoors,
			Doors redwardDoors,
			SittingDuck sittingDuck) : base(stationLocation, threatController, gravolift, bluewardDoors, redwardDoors, sittingDuck)
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
