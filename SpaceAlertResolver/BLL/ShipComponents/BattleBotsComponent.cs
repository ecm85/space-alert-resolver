using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class BattleBotsComponent : CComponent
	{
		private BattleBots battleBots = new BattleBots();
		public override CResult PerformCAction(Player performingPlayer)
		{
			if (performingPlayer.BattleBots != null)
			{
				if (performingPlayer.BattleBots.IsDisabled)
					performingPlayer.BattleBots.IsDisabled = false;
				return new CResult();
			}
			else if (battleBots != null)
			{
				performingPlayer.BattleBots = battleBots;
				battleBots = null;
				return new CResult();
			}
			return new CResult();
		}
	}
}
