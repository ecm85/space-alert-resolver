using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class Commandos : SeriousWhiteInternalThreat
	{
		protected Commandos(int timeAppears, IStation currentStation, SittingDuck sittingDuck)
			: base(2, 2, timeAppears, currentStation, PlayerAction.BattleBots, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			ChangeDecks();
		}

		public override void PerformZAction()
		{
			KnockOut(CurrentStation.Players);
			Damage(4);
		}

		public override InternalPlayerDamageResult TakeDamage(int damage, Player performingPlayer)
		{
			var result = base.TakeDamage(damage, performingPlayer);
			result.BattleBotsDisabled = true;
			return result;
		}
	}
}
