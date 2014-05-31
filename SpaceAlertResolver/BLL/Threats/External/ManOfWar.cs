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
			sittingDuck.TakeAttack(2, CurrentZone);
			speed++;
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeAttack(3, CurrentZone);
			shields++;
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeAttack(3, CurrentZone);
		}
	}
}
