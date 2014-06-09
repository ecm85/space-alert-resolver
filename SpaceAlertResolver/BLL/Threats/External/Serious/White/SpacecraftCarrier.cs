using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.White
{
	public class SpacecraftCarrier : SeriousWhiteExternalThreat
	{
		public SpacecraftCarrier()
			: base(3, 6, 2)
		{
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

		public static string GetDisplayName()
		{
			return "Spacecraft Carrier";
		}
	}
}
