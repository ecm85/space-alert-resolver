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

		public override void PerformXAction(int currentTurn)
		{
			Attack(2, ThreatDamageType.ReducedByTwoAgainstInterceptors);
		}

		public override void PerformYAction(int currentTurn)
		{
			AttackOtherTwoZones(3, ThreatDamageType.ReducedByTwoAgainstInterceptors);
		}

		public override void PerformZAction(int currentTurn)
		{
			AttackAllZones(4, ThreatDamageType.ReducedByTwoAgainstInterceptors);
		}
	}
}
