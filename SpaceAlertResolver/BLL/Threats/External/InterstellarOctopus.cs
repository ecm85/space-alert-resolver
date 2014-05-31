using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class InterstellarOctopus : SeriousWhiteExternalThreat
	{
		public InterstellarOctopus(int shields, int health, int speed, int timeAppears, Zone currentZone)
			: base(shields, health, speed, timeAppears, currentZone)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			if (IsDamaged)
				AttackAllZones(1, sittingDuck);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			if (IsDamaged)
				AttackAllZones(2, sittingDuck);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			Attack(RemainingHealth * 2);
		}

		//TODO: Cannot be targeted by rockets
	}
}
