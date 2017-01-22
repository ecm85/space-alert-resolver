namespace BLL.ShipComponents
{
	public class LowerBlueStation : LowerStation
	{
		public BatteryPack BatteryPack { get; }
		public override IAlphaComponent AlphaComponent { get; }
		public override Reactor Reactor => SideReactor;
		public SideReactor SideReactor { get; }
		protected override ICharlieComponent CharlieComponent => RocketsComponent;
		public RocketsComponent RocketsComponent { get; }

		public LowerBlueStation(
			CentralReactor whiteReactor,
			ThreatController threatController,
			Gravolift gravolift,
			Doors bluewardDoors,
			Doors redwardDoors,
			SittingDuck sittingDuck) : base(StationLocation.LowerBlue, threatController, gravolift, bluewardDoors, redwardDoors, sittingDuck)
		{
			BatteryPack = new BatteryPack();
			AlphaComponent = new SideLightLaserCannon(BatteryPack, ZoneLocation.Blue);
			RocketsComponent = new RocketsComponent();
			SideReactor = new SideReactor(whiteReactor);
		}
	}
}
