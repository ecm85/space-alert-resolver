using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Yellow
{
    public class NuclearDevice : SeriousYellowInternalThreat
    {
        internal NuclearDevice()
            : base(1, 4, StationLocation.LowerWhite, PlayerActionType.Charlie, 2)
        {
        }

        protected override void PerformXAction(int currentTurn)
        {
            Speed++;
        }

        protected override void PerformYAction(int currentTurn)
        {
            Speed++;
        }

        protected override void PerformZAction(int currentTurn)
        {
            throw new LoseException(this);
        }

        public override string Id { get; } = "SI2-05";
        public override string DisplayName { get; } = "Nuclear Device";
        public override string FileName { get; } = "NuclearDevice";
    }
}
