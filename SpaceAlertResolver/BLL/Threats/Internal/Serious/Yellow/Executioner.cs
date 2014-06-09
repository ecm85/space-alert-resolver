using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class Executioner : SeriousYellowInternalThreat
	{
		public Executioner()
			: base(2, 2, StationLocation.UpperBlue, PlayerAction.BattleBots)
		{
		}

		public static string GetDisplayName()
		{
			return "Executioner";
		}

		public override void PerformXAction(int currentTurn)
		{
			MoveRed();
			SittingDuck.KnockOutPlayersWithoutBattleBots(new [] {CurrentStation});
		}

		public override void PerformYAction(int currentTurn)
		{
			ChangeDecks();
			SittingDuck.KnockOutPlayersWithoutBattleBots(new[] { CurrentStation });
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
