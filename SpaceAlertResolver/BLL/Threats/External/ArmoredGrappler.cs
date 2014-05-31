using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class ArmoredGrappler : MinorWhiteExternalThreat
	{
		public ArmoredGrappler(int timeAppears, Zone currentZone)
			: base(3, 4, 2, timeAppears, currentZone)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			Attack(1);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			Repair(1);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			Attack(4);
		}
	}
}
