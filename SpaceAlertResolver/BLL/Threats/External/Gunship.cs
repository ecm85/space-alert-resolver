using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class Gunship : MinorWhiteExternalThreat
	{
		public Gunship(int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(2, 5, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			Attack(2);
		}

		public override void PerformYAction()
		{
			Attack(2);
		}

		public override void PerformZAction()
		{
			Attack(3);
		}
	}
}
