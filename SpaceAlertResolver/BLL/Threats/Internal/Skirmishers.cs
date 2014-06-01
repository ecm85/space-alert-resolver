using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class Skirmishers : MinorWhiteInternalThreat
	{
		protected Skirmishers(int timeAppears, IStation station, SittingDuck sittingDuck)
			: base(1, 3, timeAppears, station, PlayerAction.BattleBots, sittingDuck)
		{
		}

		public override void PerformYAction()
		{
			ChangeDecks();
		}

		public override void PerformZAction()
		{
			Damage(3);
		}

		public override InternalPlayerDamageResult TakeDamage(int damage, Player performingPlayer)
		{
			var result = base.TakeDamage(damage, performingPlayer);
			result.BattleBotsDisabled = true;
			return result;
		}
	}
}
