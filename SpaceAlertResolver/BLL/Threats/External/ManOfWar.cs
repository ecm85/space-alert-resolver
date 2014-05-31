using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class ManOfWar : SeriousWhiteExternalThreat
	{
		public ManOfWar(int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(2, 9, 1, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			Attack(2);
			speed++;
		}

		public override void PerformYAction()
		{
			Attack(3);
			shields++;
		}

		public override void PerformZAction()
		{
			Attack(3);
		}
	}
}
