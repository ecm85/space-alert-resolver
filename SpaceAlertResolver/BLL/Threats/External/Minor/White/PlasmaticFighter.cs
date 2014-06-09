using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.White
{
	public class PlasmaticFighter : MinorWhiteExternalThreat
	{
		public PlasmaticFighter()
			: base(2, 4, 3)
		{
		}

		public override void PerformXAction()
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
