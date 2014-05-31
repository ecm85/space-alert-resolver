﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public abstract class MinorWhiteExternalThreat : MinorExternalThreat
	{
		protected MinorWhiteExternalThreat(int shields, int health, int speed, int timeAppears, Zone currentZone, SittingDuck sittingDuck) :
			base(ThreatDifficulty.White, shields, health, speed, timeAppears, currentZone, sittingDuck)
		{
		}
	}
}
