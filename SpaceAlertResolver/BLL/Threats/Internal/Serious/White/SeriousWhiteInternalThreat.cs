﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.White
{
	public abstract class SeriousWhiteInternalThreat : SeriousInternalThreat
	{
		protected SeriousWhiteInternalThreat(int health, int speed, StationLocation currentStation, PlayerActionType actionType) :
			base(ThreatDifficulty.White, health, speed, currentStation, actionType)
		{
		}

		protected SeriousWhiteInternalThreat(int health, int speed, IList<StationLocation> currentStations, PlayerActionType actionType) :
			base(ThreatDifficulty.White, health, speed, currentStations, actionType)
		{
		}
	}
}
