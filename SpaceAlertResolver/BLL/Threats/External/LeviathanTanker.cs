using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class LeviathanTanker : SeriousWhiteExternalThreat
	{
		public LeviathanTanker(int timeAppears, Zone currentZone)
			: base(3, 8, 2, timeAppears, currentZone)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			Attack(2);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			Attack(2);
			Repair(2);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			Attack(2);
		}

		//TODO: Deal 1 damage to other threats on destroyed
	}
}
