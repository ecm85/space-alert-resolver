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

		public override void TakeDamage(int damage, Player performingPlayer)
		{
			base.TakeDamage(damage, performingPlayer);
			performingPlayer.BattleBots.IsDisabled = true;
		}
	}
}
