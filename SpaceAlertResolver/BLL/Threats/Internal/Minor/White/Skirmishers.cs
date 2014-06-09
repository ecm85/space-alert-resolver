using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public abstract class Skirmishers : MinorWhiteInternalThreat
	{
		protected Skirmishers(StationLocation station)
			: base(1, 3, station, PlayerAction.BattleBots)
		{
		}

		public override void PerformYAction(int currentTurn)
		{
			ChangeDecks();
		}

		public override void PerformZAction(int currentTurn)
		{
			Damage(3);
		}

		protected override void TakeDamageOnTrack(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			base.TakeDamageOnTrack(damage, performingPlayer, isHeroic, stationLocation);
			if (!isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}
	}
}
