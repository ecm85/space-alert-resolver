namespace BLL.ShipComponents
{
	public class UpperWhiteStation : UpperStation
	{
		public override IAlphaComponent AlphaComponent { get; }
		protected override Shield Shield { get; }
		protected override ICharlieComponent CharlieComponent => ComputerComponent;
		public ComputerComponent ComputerComponent { get; }

		public UpperWhiteStation(
			CentralReactor whiteReactor,
			ThreatController threatController,
			Gravolift gravolift,
			Airlock bluewardAirlock,
			Airlock redwardAirlock,
			SittingDuck sittingDuck) : base(StationLocation.LowerWhite, threatController, gravolift, bluewardAirlock, redwardAirlock, sittingDuck)
		{
			AlphaComponent = new CentralHeavyLaserCannon(whiteReactor, ZoneLocation.White);
			Shield = new CentralShield(whiteReactor);
			ComputerComponent = new ComputerComponent();
		}
	}
}
