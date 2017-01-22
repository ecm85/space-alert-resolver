namespace BLL.ShipComponents
{
	public class BlueZone : Zone
	{
		public override ZoneLocation ZoneLocation => ZoneLocation.Blue;
		public override UpperStation UpperStation => UpperBlueStation;
		public override LowerStation LowerStation => LowerBlueStation;
		public UpperBlueStation UpperBlueStation { get; set; }
		public LowerBlueStation LowerBlueStation { get; set; }

		public BlueZone(ThreatController threatController, CentralReactor whiteReactor, Doors blueDoors, SittingDuck sittingDuck)
		{
			LowerBlueStation = new LowerBlueStation(
				whiteReactor,
				threatController,
				Gravolift,
				null,
				blueDoors,
				sittingDuck);
			UpperBlueStation = new UpperBlueStation(
				LowerBlueStation.SideReactor,
				threatController,
				Gravolift,
				null,
				blueDoors,
				sittingDuck);
		}
	}
}
