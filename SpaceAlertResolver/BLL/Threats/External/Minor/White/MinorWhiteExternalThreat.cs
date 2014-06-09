using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.White
{
	public abstract class MinorWhiteExternalThreat : MinorExternalThreat
	{
		protected MinorWhiteExternalThreat(int shields, int health, int speed) :
			base(ThreatDifficulty.White, shields, health, speed)
		{
		}
	}
}
