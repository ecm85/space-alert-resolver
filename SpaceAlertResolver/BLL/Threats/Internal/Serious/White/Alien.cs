using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.White
{
	public class Alien : SeriousWhiteInternalThreat
	{
		private bool grownUp;

		public Alien()
			: base(2, 2, StationLocation.LowerWhite, PlayerAction.BattleBots)
		{
		}

		public override void PerformXAction()
		{
			grownUp = true;
		}

		public override void PerformYAction()
		{
			ChangeDecks();
			Damage(SittingDuck.GetPlayerCount(CurrentStation));
		}

		public override void PerformZAction()
		{
			throw new LoseException(this);
		}

		public static string GetDisplayName()
		{
			return "Alien";
		}

		protected override void TakeDamageOnTrack(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			base.TakeDamageOnTrack(damage, performingPlayer, isHeroic, stationLocation);
			if (grownUp && !isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}
	}
}
