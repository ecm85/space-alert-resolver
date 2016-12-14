namespace BLL.ShipComponents
{
	public class LowerWhiteStation : LowerStation
	{
		public override IAlphaComponent AlphaComponent { get; }
		public override Reactor Reactor => CentralReactor;
		public CentralReactor CentralReactor { get; }
		protected override ICharlieComponent CharlieComponent => VisualConfirmationComponent;
		public VisualConfirmationComponent VisualConfirmationComponent { get; }

		public LowerWhiteStation(
			ThreatController threatController,
			Gravolift gravolift,
			Airlock bluewardAirlock,
			Airlock redwardAirlock,
			SittingDuck sittingDuck) : base(StationLocation.LowerWhite, threatController, gravolift, bluewardAirlock, redwardAirlock, sittingDuck)
		{
			CentralReactor = new CentralReactor();
			AlphaComponent = new PulseCannon(CentralReactor);
			VisualConfirmationComponent = new VisualConfirmationComponent();
		}
	}
}
