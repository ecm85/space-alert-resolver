namespace BLL.ShipComponents
{
    public class UpperWhiteStation : UpperStation
    {
        public override IAlphaComponent AlphaComponent { get; }
        protected override Shield Shield { get; }
        protected override ICharlieComponent CharlieComponent => ComputerComponent;
        public ComputerComponent ComputerComponent { get; }

        internal UpperWhiteStation(
            CentralReactor whiteReactor,
            ThreatController threatController,
            Gravolift gravolift,
            Doors bluewardDoors,
            Doors redwardDoors,
            SittingDuck sittingDuck) : base(StationLocation.UpperWhite, threatController, gravolift, bluewardDoors, redwardDoors, sittingDuck)
        {
            AlphaComponent = new CentralHeavyLaserCannon(whiteReactor, ZoneLocation.White);
            Shield = new CentralShield(whiteReactor);
            ComputerComponent = new ComputerComponent();
        }
    }
}
