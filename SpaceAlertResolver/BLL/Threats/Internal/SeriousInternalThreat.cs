using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class SeriousInternalThreat : InternalThreat
	{
		protected SeriousInternalThreat(ThreatDifficulty difficulty, int health, int speed, int timeAppears, IStation currentStation, PlayerAction actionType, SittingDuck sittingDuck)
			: base(ThreatType.SeriousInternal, difficulty, health, speed, timeAppears, currentStation, actionType, sittingDuck)
		{
		}

		protected SeriousInternalThreat(ThreatDifficulty difficulty, int health, int speed, int timeAppears, IList<IStation> currentStations, PlayerAction actionType, SittingDuck sittingDuck)
			: base(ThreatType.SeriousInternal, difficulty, health, speed, timeAppears, currentStations, actionType, sittingDuck)
		{
		}
	}
}
