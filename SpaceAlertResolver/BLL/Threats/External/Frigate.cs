using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class Frigate : SeriousWhiteExternalThreat
	{
		public Frigate(int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(2, 7, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			Attack(2);
		}

		public override void PerformYAction()
		{
			Attack(3);
		}

		public override void PerformZAction()
		{
			Attack(4);
		}
	}
}
