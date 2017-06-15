using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow.Slime
{
    public class SlimeB : NormalSlime
    {
        internal SlimeB()
            : base(StationLocation.LowerRed)
        {
        }

        protected override void PerformXAction(int currentTurn)
        {
            SittingDuck.DisableLowerRedInactiveBattleBots();
        }

        public override string Id { get; } = "I2-02";
        public override string DisplayName { get; } = "Slime";
        public override string FileName { get; } = "SlimeB";

        protected override ProgenySlime CreateProgeny(StationLocation stationLocation)
        {
            return new ProgenySlimeB(this, stationLocation);
        }

        protected override StationLocation? GetStationToSpreadTo(StationLocation stationLocation)
        {
            return stationLocation.BluewardStationLocation();
        }
    }
}
