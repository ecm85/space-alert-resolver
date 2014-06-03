using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class ArmoredGrappler : MinorWhiteExternalThreat
	{
		public ArmoredGrappler(int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(3, 4, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			Attack(1);
		}

		public override void PerformYAction()
		{
			Repair(1);
		}

		public override void PerformZAction()
		{
			Attack(4);
		}

		public static string GetDisplayName()
		{
			return "Armored Grappler";
		}
	}
}
