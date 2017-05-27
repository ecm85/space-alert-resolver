using BLL.Common;
using BLL.Players;

namespace BLL.ShipComponents
{
	public class BattleBotsComponent : ICharlieComponent
	{
		public BattleBots BattleBots { get; private set; } = new BattleBots();

		public void DisableInactiveBattleBots()
		{
			if (BattleBots != null)
				BattleBots.IsDisabled = true;
		}

		public void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvancedUsage)
		{
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
			if (performingPlayer.BattleBots != null)
			{
				if (performingPlayer.BattleBots.IsDisabled)
					performingPlayer.BattleBots.IsDisabled = false;
			}
			else if (BattleBots != null)
			{
				performingPlayer.BattleBots = BattleBots;
				BattleBots = null;
			}
		}

		public bool CanPerformCAction(Player performingPlayer)
		{
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
			return performingPlayer.BattleBots != null || BattleBots != null;
		}
	}
}
