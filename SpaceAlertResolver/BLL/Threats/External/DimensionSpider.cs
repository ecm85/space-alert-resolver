using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class DimensionSpider : SeriousWhiteExternalThreat
	{
		public DimensionSpider(int speed, int timeAppears, Zone currentZone)
			: base(0, 13, speed, timeAppears, currentZone)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			shields = 1;
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			shields++;
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeAttack(4, sittingDuck.Zones);
		}

		public override void JumpingToHyperspace(SittingDuck sittingDuck)
		{
			PerformZAction(sittingDuck);
		}
		a
		//TODO: Cannot be targeted by rockets
	}
}
