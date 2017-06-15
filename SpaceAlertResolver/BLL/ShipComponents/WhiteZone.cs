namespace BLL.ShipComponents
{
    public class WhiteZone : Zone
    {
        public override ZoneLocation ZoneLocation => ZoneLocation.White;
        public override UpperStation UpperStation => UpperWhiteStation;
        public override LowerStation LowerStation => LowerWhiteStation;
        public UpperWhiteStation UpperWhiteStation { get; set; }
        public LowerWhiteStation LowerWhiteStation { get; set; }

        internal WhiteZone(ThreatController threatController, Doors redDoors, Doors blueDoors, SittingDuck sittingDuck)
        {
            LowerWhiteStation = new LowerWhiteStation(
                threatController,
                Gravolift,
                blueDoors,
                redDoors,
                sittingDuck);
            UpperWhiteStation = new UpperWhiteStation(
                LowerWhiteStation.CentralReactor,
                threatController,
                Gravolift,
                redDoors,
                redDoors,
                sittingDuck);
        }
    }
}
