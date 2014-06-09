using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.Yellow
{
	public class MiniCarrier : MinorYellowExternalThreat
	{
		public MiniCarrier()
			: base(2, 5, 2)
		{
		}

		public static string GetDisplayName()
		{
			return "Mini-Carrier";
		}

		public override void PerformXAction()
		{
			Attack(2, ThreatDamageType.ReducedByTwoAgainstInterceptors);
		}

		public override void PerformYAction()
		{
			AttackOtherTwoZones(3, ThreatDamageType.ReducedByTwoAgainstInterceptors);
		}

		public override void PerformZAction()
		{
			AttackAllZones(4, ThreatDamageType.ReducedByTwoAgainstInterceptors);
		}
	}
}
