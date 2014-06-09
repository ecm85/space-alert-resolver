using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Yellow
{
	public class PlasmaticFrigate : SeriousYellowExternalThreat
	{
		public PlasmaticFrigate()
			: base(2, 7, 2)
		{
		}

		public override void PerformXAction()
		{
			Attack(2, ThreatDamageType.Plasmatic);
		}

		public override void PerformYAction()
		{
			Attack(2, ThreatDamageType.Plasmatic);
		}

		public override void PerformZAction()
		{
			Attack(4, ThreatDamageType.Plasmatic);
		}

		public static string GetDisplayName()
		{
			return "Plasmatic Frigate";
		}
	}
}
