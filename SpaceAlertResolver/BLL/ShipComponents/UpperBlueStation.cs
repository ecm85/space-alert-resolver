namespace BLL.ShipComponents
{
    public class UpperBlueStation : UpperStation
    {
        public override IAlphaComponent AlphaComponent { get; }
        protected override Shield Shield { get; }
        protected override ICharlieComponent CharlieComponent => BattleBotsComponent;
        public BattleBotsComponent BattleBotsComponent { get; }

        internal UpperBlueStation(
            SideReactor redReactor,
            ThreatController threatController,
            Gravolift gravolift,
            Doors bluewardDoors,
            Doors redwardDoors,
            SittingDuck sittingDuck) : base(StationLocation.UpperBlue, threatController, gravolift, bluewardDoors, redwardDoors, sittingDuck)
        {
            AlphaComponent = new SideHeavyLaserCannon(redReactor, ZoneLocation.Blue);
            Shield = new SideShield(redReactor);
            BattleBotsComponent = new BattleBotsComponent();
        }
    }
}
