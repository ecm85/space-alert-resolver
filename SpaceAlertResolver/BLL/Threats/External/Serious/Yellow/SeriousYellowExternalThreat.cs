using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Yellow
{
	public abstract class SeriousYellowExternalThreat : SeriousExternalThreat
	{
		protected SeriousYellowExternalThreat(int shields, int health, int speed)
			: base(ThreatDifficulty.Yellow, shields, health, speed)
		{
		}
	}
}
