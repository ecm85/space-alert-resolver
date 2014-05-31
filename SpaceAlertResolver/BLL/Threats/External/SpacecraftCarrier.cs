using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class SpacecraftCarrier : SeriousWhiteExternalThreat
	{
		public SpacecraftCarrier(int timeAppears, Zone currentZone)
			: base(3, 6, 2, timeAppears, currentZone)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			Attack(2);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			AttackOtherTwoZones(3, sittingDuck);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			AttackAllZones(4, sittingDuck);
		}

		//TODO: Reduce attack if interceptors are out
	}
}
