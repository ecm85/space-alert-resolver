using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class Fighter : MinorWhiteExternalThreat
	{
		public Fighter(int timeAppears, Zone currentZone)
			: base(2, 4, 3, timeAppears, currentZone)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			Attack(1);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			Attack(2);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			Attack(3);
		}
	}
}
