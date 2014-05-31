using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class SeriousWhiteInternalThreat : SeriousInternalThreat
	{
		protected SeriousWhiteInternalThreat(int health, int speed, int timeAppears, Station currentStation, PlayerAction actionType, SittingDuck sittingDuck) :
			base(4, 8, health, speed, timeAppears, currentStation, actionType, sittingDuck)
		{
		}
	}
}
