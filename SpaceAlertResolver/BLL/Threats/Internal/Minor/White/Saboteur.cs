﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public abstract class Saboteur : MinorWhiteInternalThreat
	{
		protected Saboteur()
			: base(1, 4, StationLocation.LowerWhite, PlayerAction.BattleBots)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
			var energyDrained = SittingDuck.DrainReactors(CurrentZones, 1);
			if (energyDrained == 0)
				Damage(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(2);
		}
	}
}