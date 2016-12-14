namespace BLL.ShipComponents
{
	public class RedZone : Zone
	{
		public override ZoneLocation ZoneLocation => ZoneLocation.Red;
		public override UpperStation UpperStation => UpperRedStation;
		public override LowerStation LowerStation => LowerRedStation;
		public UpperRedStation UpperRedStation { get; set; }
		public LowerRedStation LowerRedStation { get; set; }

		public RedZone(ThreatController threatController, CentralReactor whiteReactor, Airlock redAirlock, SittingDuck sittingDuck, Interceptors interceptors)
		{
			LowerRedStation = new LowerRedStation(
				whiteReactor,
				threatController,
				Gravolift,
				redAirlock,
				null,
				sittingDuck);
			UpperRedStation = new UpperRedStation(
				LowerRedStation.SideReactor,
				interceptors,
				threatController,
				Gravolift,
				redAirlock,
				null,
				sittingDuck);
		}
	}
}
