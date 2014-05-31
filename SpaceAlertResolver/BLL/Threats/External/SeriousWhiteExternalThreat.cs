using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public abstract class SeriousWhiteExternalThreat : SeriousExternalThreat
	{
		protected SeriousWhiteExternalThreat(int shields, int health, int speed, int timeAppears, Zone currentZone, SittingDuck sittingDuck) :
			base(4, 8, shields, health, speed, timeAppears, currentZone, sittingDuck)
		{
		}
	}
}
