using BLL.Common;
using BLL.Players;

namespace BLL.ShipComponents
{
    public class InterceptorsOnShipComponent : ICharlieComponent
    {
        private readonly SittingDuck sittingDuck;
        public Interceptors Interceptors { get; private set; }

        private Station SpacewardStation => sittingDuck.StationsByLocation[StationLocation.Interceptor1];

        internal InterceptorsOnShipComponent(
            SittingDuck sittingDuck,
            Interceptors interceptors)
        {
            Interceptors = interceptors;
            this.sittingDuck = sittingDuck;
        }

        public void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvancedUsage)
        {
            Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
            LaunchInterceptors(performingPlayer);
            performingPlayer.CurrentStation.Players.Remove(performingPlayer);
            SpacewardStation.MovePlayerIn(performingPlayer, currentTurn);
        }

        private void LaunchInterceptors(Player performingPlayer)
        {
            performingPlayer.Interceptors = Interceptors;
            Interceptors = null;
        }

        public bool CanPerformCAction(Player performingPlayer)
        {
            return Interceptors != null && performingPlayer.BattleBots != null && !performingPlayer.BattleBots.IsDisabled;
        }

        public void DockInterceptors(Player performingPlayer)
        {
            Interceptors = performingPlayer.Interceptors;
            performingPlayer.Interceptors = null;
        }
    }
}
