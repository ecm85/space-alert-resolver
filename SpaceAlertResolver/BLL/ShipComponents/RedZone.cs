namespace BLL.ShipComponents
{
    public class RedZone : Zone
    {
        public override ZoneLocation ZoneLocation => ZoneLocation.Red;
        public override UpperStation UpperStation => UpperRedStation;
        public override LowerStation LowerStation => LowerRedStation;
        public UpperRedStation UpperRedStation { get; set; }
        public LowerRedStation LowerRedStation { get; set; }

        internal RedZone(ThreatController threatController, CentralReactor whiteReactor, Doors redDoors, SittingDuck sittingDuck, Interceptors interceptors)
        {
            LowerRedStation = new LowerRedStation(
                whiteReactor,
                threatController,
                Gravolift,
                redDoors,
                null,
                sittingDuck);
            UpperRedStation = new UpperRedStation(
                LowerRedStation.SideReactor,
                interceptors,
                threatController,
                Gravolift,
                redDoors,
                null,
                sittingDuck);
        }
    }
}
