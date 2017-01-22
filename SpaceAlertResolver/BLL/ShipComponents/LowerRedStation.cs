namespace BLL.ShipComponents
{
	public class LowerRedStation : LowerStation
	{
		public BatteryPack BatteryPack { get; }
		public override IAlphaComponent AlphaComponent { get; }
		public override Reactor Reactor => SideReactor;
		public SideReactor SideReactor { get; }
		protected override ICharlieComponent CharlieComponent => BattleBotsComponent;
		public BattleBotsComponent BattleBotsComponent { get; }

		public LowerRedStation(
			CentralReactor whiteReactor,
			ThreatController threatController,
			Gravolift gravolift,
			Doors bluewardDoors,
			Doors redwardDoors,
			SittingDuck sittingDuck) : base(StationLocation.LowerRed, threatController, gravolift, bluewardDoors, redwardDoors, sittingDuck)
		{
			BatteryPack = new BatteryPack();
			AlphaComponent = new SideLightLaserCannon(BatteryPack, ZoneLocation.Red);
			BattleBotsComponent = new BattleBotsComponent();
			SideReactor = new SideReactor(whiteReactor);
		}
	}
}
