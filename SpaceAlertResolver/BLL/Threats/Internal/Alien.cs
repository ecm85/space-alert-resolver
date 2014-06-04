using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class Alien : SeriousWhiteInternalThreat
	{
		private bool grownUp;

		public Alien(int timeAppears, ISittingDuck sittingDuck)
			: base(2, 2, timeAppears, StationLocation.LowerWhite, PlayerAction.BattleBots, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			grownUp = true;
		}

		public override void PerformYAction()
		{
			ChangeDecks();
			Damage(sittingDuck.GetPlayerCount(CurrentStation));
		}

		public override void PerformZAction()
		{
			throw new LoseException(this);
		}

		public static string GetDisplayName()
		{
			return "Alien";
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic);
			if (grownUp && !isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}
	}
}
