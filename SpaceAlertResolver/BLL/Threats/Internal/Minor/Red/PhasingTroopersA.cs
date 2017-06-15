using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Red
{
    public class PhasingTroopersA : PhasingTroopers
    {
        internal PhasingTroopersA() : base(StationLocation.LowerBlue)
        {
        }

        protected override void PerformYAction(int currentTurn)
        {
            MoveRed();
        }

        public override string Id { get; } = "I3-106";
        public override string DisplayName { get; } = "Phasing Troopers";
        public override string FileName { get; } = "PhasingTroopersA";
    }
}
