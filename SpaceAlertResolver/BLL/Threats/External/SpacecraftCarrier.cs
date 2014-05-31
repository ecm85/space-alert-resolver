using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class SpacecraftCarrier : SeriousWhiteExternalThreat
	{
		public SpacecraftCarrier(int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(3, 6, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			Attack(2);
		}

		public override void PerformYAction()
		{
			AttackOtherTwoZones(3);
		}

		public override void PerformZAction()
		{
			AttackAllZones(4);
		}

		//TODO: Reduce attack if interceptors are out
	}
}
