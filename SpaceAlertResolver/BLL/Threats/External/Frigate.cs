using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class Frigate : SeriousWhiteExternalThreat
	{
		public Frigate(int timeAppears, Zone currentZone)
			: base(2, 7, 2, timeAppears, currentZone)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			Attack(2);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			Attack(3);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			Attack(4);
		}
	}
}
