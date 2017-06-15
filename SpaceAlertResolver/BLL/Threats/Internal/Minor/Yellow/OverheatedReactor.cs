using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
    public class OverheatedReactor : MinorYellowInternalThreat
    {
        internal OverheatedReactor()
            : base(3, 2, StationLocation.LowerWhite, PlayerActionType.Bravo)
        {
        }
        protected override void PerformXAction(int currentTurn)
        {
            Attack(SittingDuck.GetEnergyInReactor(CurrentZone));
        }

        protected override void PerformYAction(int currentTurn)
        {
            SittingDuck.DestroyFuelCapsule();
        }

        protected override void PerformZAction(int currentTurn)
        {
            Attack(3);
        }

        protected override void OnHealthReducedToZero()
        {
            base.OnHealthReducedToZero();
            SittingDuck.KnockOutPlayers(new [] {StationLocation.LowerBlue, StationLocation.LowerRed});
        }

        public override string Id { get; } = "I2-06";
        public override string DisplayName { get; } = "Overheated Reactor";
        public override string FileName { get; } = "OverheatedReactor";
    }
}
