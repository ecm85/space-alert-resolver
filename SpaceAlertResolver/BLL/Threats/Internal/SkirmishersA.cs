﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class SkirmishersA : MinorWhiteInternalThreat
	{
		protected SkirmishersA(int timeAppears, SittingDuck sittingDuck)
			: base(1, 3, timeAppears, sittingDuck.RedZone.UpperStation, PlayerAction.BattleBots, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			MoveRed();
		}

		public override void PerformYAction()
		{
			ChangeDecks();
		}

		public override void PerformZAction()
		{
			sittingDuck.TakeDamage(3, CurrentStation.ZoneLocation);
		}

		public override InternalPlayerDamageResult TakeDamage(int damage)
		{
			var result = base.TakeDamage(damage);
			result.BattleBotsDisabled = true;
			return result;
		}
	}
}
