using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
    public class SkirmishersA : Skirmishers
    {
        internal SkirmishersA()
            : base(StationLocation.UpperRed)
        {
        }

        protected override void PerformXAction(int currentTurn)
        {
            MoveBlue();
        }

        public override string Id { get; } = "I1-01";
        public override string DisplayName { get; } = "Skirmishers";
        public override string FileName { get; } = "SkirmishersA";
    }
}
