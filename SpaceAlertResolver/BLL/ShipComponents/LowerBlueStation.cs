namespace BLL.ShipComponents
{
	public class LowerBlueStation : LowerStation
	{
		public override IAlphaComponent AlphaComponent { get; }
		public override Reactor Reactor => SideReactor;
		public SideReactor SideReactor { get; }
		protected override ICharlieComponent CharlieComponent => RocketsComponent;
		public RocketsComponent RocketsComponent { get; }

		public LowerBlueStation(
			CentralReactor whiteReactor,
			ThreatController threatController,
			Gravolift gravolift,
			Airlock bluewardAirlock,
			Airlock redwardAirlock,
			SittingDuck sittingDuck) : base(StationLocation.LowerBlue, threatController, gravolift, bluewardAirlock, redwardAirlock, sittingDuck)
		{
			AlphaComponent = new SideLightLaserCannon(new BatteryPack(), ZoneLocation.Blue);
			RocketsComponent = new RocketsComponent();
			SideReactor = new SideReactor(whiteReactor);
		}
	}
}
