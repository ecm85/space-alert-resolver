using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public abstract class Troopers : MinorYellowInternalThreat
	{
		protected Troopers(StationLocation currentStation)
			: base(2, 2, currentStation, PlayerAction.BattleBots)
		{
		}

		public override void PerformXAction(int currentTurn)
		{
			ChangeDecks();
		}

		public override void PerformZAction(int currentTurn)
		{
			Damage(4);
		}

		protected override void TakeDamageOnTrack(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			base.TakeDamageOnTrack(damage, performingPlayer, isHeroic, stationLocation);
			if (!isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}
	}
}
