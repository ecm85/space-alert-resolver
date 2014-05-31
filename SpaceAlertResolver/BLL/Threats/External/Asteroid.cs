using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class Asteroid : SeriousWhiteExternalThreat
	{
		private int breakpointsCrossed;

		public Asteroid(int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(0, 9, 3, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			breakpointsCrossed++;
		}

		public override void PerformYAction()
		{
			breakpointsCrossed++;
		}

		public override void PerformZAction()
		{
			Attack(RemainingHealth);
		}

		//TODO: Cannot be targeted by rockets
		//TODO: Deal 2 damage per breakpoint crossed on destroyed
	}
}
