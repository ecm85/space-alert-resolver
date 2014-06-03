using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class PlasmaticFighter : MinorWhiteExternalThreat
	{
		public PlasmaticFighter(int timeAppears, ZoneLocation currentZone, SittingDuck sittingDuck)
			: base(2, 4, 3, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			Attack(1, ThreatDamageType.Plasmatic);
		}

		public override void PerformYAction()
		{
			Attack(1, ThreatDamageType.Plasmatic);
		}

		public override void PerformZAction()
		{
			Attack(2, ThreatDamageType.Plasmatic);
		}

		public static string GetDisplayName()
		{
			return "Plasmatic Fighter";
		}
	}
}
