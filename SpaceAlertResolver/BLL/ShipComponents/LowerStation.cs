namespace BLL.ShipComponents
{
    public abstract class LowerStation : StandardStation
    {
        public abstract Reactor Reactor { get; }

        public override IBravoComponent BravoComponent => Reactor;

        protected LowerStation(
            StationLocation stationLocation,
            ThreatController threatController,
            Gravolift gravolift,
            Doors bluewardDoors,
            Doors redwardDoors,
            SittingDuck sittingDuck) : base(stationLocation, threatController, gravolift, bluewardDoors, redwardDoors, sittingDuck)
        {
        }

        public override void DrainEnergy(int? amount)
        {
            DrainReactor(amount);
        }

        public int DrainReactor(int? amount)
        {
            return Reactor.Drain(amount ?? Reactor.Energy);
        }

        public int EnergyInReactor => Reactor.Energy;
    }
}
