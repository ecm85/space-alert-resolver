using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
	public abstract class MinorWhiteInternalThreat : MinorInternalThreat
	{
		protected MinorWhiteInternalThreat(int health, int speed, StationLocation currentStation, PlayerActionType actionType, int? accessibility = null) :
			base(ThreatDifficulty.White, health, speed, currentStation, actionType, accessibility)
		{
		}
	}
}
