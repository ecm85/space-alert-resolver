using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class ManOfWar : SeriousWhiteExternalThreat
	{
		public ManOfWar(int timeAppears, Zone currentZone)
			: base(2, 9, 1, timeAppears, currentZone)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			Attack(2);
			speed++;
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			Attack(3);
			shields++;
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			Attack(3);
		}
	}
}
