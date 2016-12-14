namespace BLL.ShipComponents
{
	public class WhiteZone : Zone
	{
		public override ZoneLocation ZoneLocation => ZoneLocation.White;
		public override UpperStation UpperStation => UpperWhiteStation;
		public override LowerStation LowerStation => LowerWhiteStation;
		public UpperWhiteStation UpperWhiteStation { get; set; }
		public LowerWhiteStation LowerWhiteStation { get; set; }

		public WhiteZone(ThreatController threatController, Airlock redAirlock, Airlock blueAirlock, SittingDuck sittingDuck)
		{
			LowerWhiteStation = new LowerWhiteStation(
				threatController,
				Gravolift,
				blueAirlock,
				redAirlock,
				sittingDuck);
			UpperWhiteStation = new UpperWhiteStation(
				LowerWhiteStation.CentralReactor,
				threatController,
				Gravolift,
				redAirlock,
				redAirlock,
				sittingDuck);
		}
	}
}
