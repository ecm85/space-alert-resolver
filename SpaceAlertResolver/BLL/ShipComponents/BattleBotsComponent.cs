using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Common;

namespace BLL.ShipComponents
{
	public class BattleBotsComponent : ICharlieComponent
	{
		private BattleBots battleBots = new BattleBots();

		public void DisableInactiveBattleBots()
		{
			if (battleBots != null)
				battleBots.IsDisabled = true;
		}

		public void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvancedUsage)
		{
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
			if (performingPlayer.BattleBots != null)
			{
				if (performingPlayer.BattleBots.IsDisabled)
					performingPlayer.BattleBots.IsDisabled = false;
			}
			else if (battleBots != null)
			{
				performingPlayer.BattleBots = battleBots;
				battleBots = null;
			}
		}

		public bool CanPerformCAction(Player performingPlayer)
		{
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
			return performingPlayer.BattleBots != null || battleBots != null;
		}
	}
}
