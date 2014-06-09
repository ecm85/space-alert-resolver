using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.White
{
	public abstract class Commandos : SeriousWhiteInternalThreat
	{
		protected Commandos(StationLocation currentStation)
			: base(2, 2, currentStation, PlayerAction.BattleBots)
		{
		}

		public override void PerformXAction(int currentTurn)
		{
			ChangeDecks();
		}

		public override void PerformZAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayers(new [] {CurrentStation});
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
