namespace BLL.ShipComponents
{
	public class LowerRedStation : LowerStation
	{
		public override IAlphaComponent AlphaComponent { get; }
		public override Reactor Reactor => SideReactor;
		public SideReactor SideReactor { get; }
		protected override ICharlieComponent CharlieComponent => BattleBotsComponent;
		public BattleBotsComponent BattleBotsComponent { get; }

		public LowerRedStation(
			CentralReactor whiteReactor,
			ThreatController threatController,
			Gravolift gravolift,
			Airlock bluewardAirlock,
			Airlock redwardAirlock,
			SittingDuck sittingDuck) : base(StationLocation.LowerRed, threatController, gravolift, bluewardAirlock, redwardAirlock, sittingDuck)
		{
			AlphaComponent = new SideLightLaserCannon(new BatteryPack(), ZoneLocation.Red);
			BattleBotsComponent = new BattleBotsComponent();
			SideReactor = new SideReactor(whiteReactor);
		}
	}
}
