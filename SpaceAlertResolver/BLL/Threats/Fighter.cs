using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats
{
	public class Fighter : MinorExternalThreat
	{
		public Fighter(int timeAppears, ZoneType currentZoneType)
			: base(2, 4, 2, 4, 3, timeAppears, currentZoneType)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeDamage(1, CurrentZoneType);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeDamage(2, CurrentZoneType);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeDamage(3, CurrentZoneType);
		}
	}
}
