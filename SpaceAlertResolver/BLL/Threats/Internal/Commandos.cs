using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class Commandos : SeriousWhiteInternalThreat
	{
		protected Commandos(int timeAppears, Station currentStation, SittingDuck sittingDuck)
			: base(2, 2, timeAppears, currentStation, PlayerAction.BattleBots, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			ChangeDecks();
		}

		public override void PerformZAction()
		{
			foreach (var player in CurrentStation.Players)
				player.IsKnockedOut = true;
			sittingDuck.TakeDamage(4, CurrentStation.ZoneLocation);
		}

		public override InternalPlayerDamageResult TakeDamage(int damage)
		{
			var result = base.TakeDamage(damage);
			result.BattleBotsDisabled = true;
			return result;
		}
	}
}
