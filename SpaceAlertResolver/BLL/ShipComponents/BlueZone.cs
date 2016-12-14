namespace BLL.ShipComponents
{
	public class BlueZone : Zone
	{
		public override ZoneLocation ZoneLocation => ZoneLocation.Blue;
		public override UpperStation UpperStation => UpperBlueStation;
		public override LowerStation LowerStation => LowerBlueStation;
		public UpperBlueStation UpperBlueStation { get; set; }
		public LowerBlueStation LowerBlueStation { get; set; }

		public BlueZone(ThreatController threatController, CentralReactor whiteReactor, Airlock blueAirlock, SittingDuck sittingDuck)
		{
			LowerBlueStation = new LowerBlueStation(
				whiteReactor,
				threatController,
				Gravolift,
				null,
				blueAirlock,
				sittingDuck);
			UpperBlueStation = new UpperBlueStation(
				LowerBlueStation.SideReactor,
				threatController,
				Gravolift,
				null,
				blueAirlock,
				sittingDuck);
		}
	}
}
