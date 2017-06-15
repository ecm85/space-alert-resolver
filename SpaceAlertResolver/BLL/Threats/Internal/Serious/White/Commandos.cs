using BLL.Common;
using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.White
{
    public abstract class Commandos : SeriousWhiteInternalThreat
    {
        protected Commandos(StationLocation currentStation)
            : base(2, 2, currentStation, PlayerActionType.BattleBots)
        {
        }

        protected override void PerformXAction(int currentTurn)
        {
            ChangeDecks();
        }

        protected override void PerformZAction(int currentTurn)
        {
            SittingDuck.KnockOutPlayers(new [] {CurrentStation});
            Attack(4);
        }

        public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
        {
            Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
            base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
            if (!isHeroic)
                performingPlayer.BattleBots.IsDisabled = true;
        }
    }
}
