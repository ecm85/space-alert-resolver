using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class MinorWhiteInternalThreat : MinorInternalThreat
	{
		protected MinorWhiteInternalThreat(int health, int speed, int timeAppears, Station currentStation, PlayerAction actionType, SittingDuck sittingDuck) :
			base(2, 4, health, speed, timeAppears, currentStation, actionType, sittingDuck)
		{
		}
	}
}
