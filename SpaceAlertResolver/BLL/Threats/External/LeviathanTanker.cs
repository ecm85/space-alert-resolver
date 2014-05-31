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
			sittingDuck.TakeAttack(2, CurrentZone);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeAttack(2, CurrentZone);
			Repair(2);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeAttack(2, CurrentZone);
		}

		//TODO: Deal 1 damage to other threats on destroyed
	}
}
