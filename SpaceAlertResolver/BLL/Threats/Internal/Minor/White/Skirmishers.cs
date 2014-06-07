using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public abstract class Skirmishers : MinorWhiteInternalThreat
	{
		protected Skirmishers(int timeAppears, StationLocation station, ISittingDuck sittingDuck)
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

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (!isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}
	}
}
