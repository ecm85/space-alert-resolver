namespace BLL.ShipComponents
{
	public class UpperRedStation : UpperStation
	{
		public override IAlphaComponent AlphaComponent { get; }
		protected override Shield Shield { get; }
		protected override ICharlieComponent CharlieComponent => InterceptorComponent;
		public InterceptorsOnShipComponent InterceptorComponent { get; }

		public UpperRedStation(
			SideReactor redReactor,
			Interceptors interceptors,
			ThreatController threatController,
			Gravolift gravolift,
			Airlock bluewardAirlock,
			Airlock redwardAirlock,
			SittingDuck sittingDuck) : base(StationLocation.UpperRed, threatController, gravolift, bluewardAirlock, redwardAirlock, sittingDuck)
		{
			AlphaComponent = new SideHeavyLaserCannon(redReactor, ZoneLocation.Red);
			Shield = new SideShield(redReactor);
			InterceptorComponent = new InterceptorsOnShipComponent(sittingDuck, interceptors);
		}

		public override void MovePlayerIn(Player performingPlayer, int currentTurn)
		{
			base.MovePlayerIn(performingPlayer, currentTurn);
			if (performingPlayer.Interceptors != null)
			{
				InterceptorComponent.DockInterceptors(performingPlayer);
			}
		}
	}
}
