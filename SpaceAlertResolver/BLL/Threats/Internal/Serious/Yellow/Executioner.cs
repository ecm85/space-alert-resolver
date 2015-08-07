﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Common;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class Executioner : SeriousYellowInternalThreat
	{
		public Executioner()
			: base(2, 2, StationLocation.UpperBlue, PlayerActionType.BattleBots)
		{
		}
		protected override void PerformXAction(int currentTurn)
		{
			MoveRed();
			SittingDuck.KnockOutPlayersWithoutBattleBots(new [] {CurrentStation});
		}

		protected override void PerformYAction(int currentTurn)
		{
			ChangeDecks();
			SittingDuck.KnockOutPlayersWithoutBattleBots(new[] { CurrentStation });
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(3);
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (!isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}
	}
}
