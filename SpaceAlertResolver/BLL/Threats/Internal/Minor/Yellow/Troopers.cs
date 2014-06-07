using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public abstract class Troopers : MinorYellowInternalThreat
	{
		protected Troopers(int timeAppears, StationLocation currentStation, ISittingDuck sittingDuck)
			: base(2, 2, timeAppears, currentStation, PlayerAction.BattleBots, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			ChangeDecks();
		}

		public override void PerformZAction()
		{
			Damage(4);
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (!isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}
	}
}
