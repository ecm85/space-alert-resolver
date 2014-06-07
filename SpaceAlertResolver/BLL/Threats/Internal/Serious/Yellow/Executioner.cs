using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class Executioner : SeriousYellowInternalThreat
	{
		public Executioner(int timeAppears, ISittingDuck sittingDuck)
			: base(2, 2, timeAppears, StationLocation.UpperBlue, PlayerAction.BattleBots, sittingDuck)
		{
		}

		public static string GetDisplayName()
		{
			return "Executioner";
		}

		public override void PeformXAction()
		{
			MoveRed();
			sittingDuck.KnockOutPlayersWithoutBattleBots(new [] {CurrentStation});
		}

		public override void PerformYAction()
		{
			ChangeDecks();
			sittingDuck.KnockOutPlayersWithoutBattleBots(new[] { CurrentStation });
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
