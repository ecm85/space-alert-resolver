namespace BLL.ShipComponents
{
	public class UpperRedStation : UpperStation
	{
		public override IAlphaComponent AlphaComponent { get; }
		protected override Shield Shield { get; }
		protected override ICharlieComponent CharlieComponent => InterceptorComponent;
		public InterceptorComponent InterceptorComponent { get; }

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
			InterceptorComponent = new InterceptorComponent(sittingDuck, interceptors, StationLocation.UpperRed);
		}
	}
}
