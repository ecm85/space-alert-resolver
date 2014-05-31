using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class DimensionSpider : SeriousWhiteExternalThreat
	{
		public DimensionSpider(int speed, int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(0, 13, speed, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			shields = 1;
		}

		public override void PerformYAction()
		{
			shields++;
		}

		public override void PerformZAction()
		{
			AttackAllZones(4);
		}

		public override void JumpingToHyperspace()
		{
			PerformZAction();
		}
		
		//TODO: Cannot be targeted by rockets
	}
}
