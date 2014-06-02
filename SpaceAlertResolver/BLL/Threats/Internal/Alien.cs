using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class Alien : SeriousWhiteInternalThreat
	{
		private bool grownUp;

		public Alien(int timeAppears, SittingDuck sittingDuck)
			: base(2, 2, timeAppears, sittingDuck.WhiteZone.LowerStation, PlayerAction.BattleBots, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			grownUp = true;
		}

		public override void PerformYAction()
		{
			ChangeDecks();
			Damage(CurrentStation.Players.Count);
		}

		public override void PerformZAction()
		{
			throw new LoseException(this);
		}

		public override void TakeDamage(int damage, Player performingPlayer)
		{
			base.TakeDamage(damage, performingPlayer);
			if (grownUp)
				performingPlayer.BattleBots.IsDisabled = true;
		}
	}
}
