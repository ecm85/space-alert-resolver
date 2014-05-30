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
			sittingDuck.TakeAttack(1, CurrentZone);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeAttack(2, CurrentZone);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeAttack(3, CurrentZone);
		}
	}
}
