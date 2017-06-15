using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.White
{
    public class Shambler : SeriousWhiteInternalThreat
    {
        internal Shambler()
            : base(2, 2, StationLocation.LowerWhite, PlayerActionType.BattleBots)
        {
        }

        protected override void PerformXAction(int currentTurn)
        {
            if (IsAnyPlayerPresent())
                MoveBlue();
        }

        protected override void PerformYAction(int currentTurn)
        {
            if (IsAnyPlayerPresent())
                Attack(2);
            else
                Repair(1);
        }

        protected override void PerformZAction(int currentTurn)
        {
            Attack(4);
        }

        public override string Id { get; } = "SI1-101";
        public override string DisplayName { get; } = "Shambler";
        public override string FileName { get; } = "Shambler";

        private bool IsAnyPlayerPresent()
        {
            return SittingDuck.GetPlayerCount(CurrentStation) != 0;
        }
    }
}
