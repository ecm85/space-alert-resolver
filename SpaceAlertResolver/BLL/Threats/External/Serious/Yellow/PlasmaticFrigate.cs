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

		public override void PerformXAction(int currentTurn)
		{
			Attack(2, ThreatDamageType.Plasmatic);
		}

		public override void PerformYAction(int currentTurn)
		{
			Attack(2, ThreatDamageType.Plasmatic);
		}

		public override void PerformZAction(int currentTurn)
		{
			Attack(4, ThreatDamageType.Plasmatic);
		}

		public static string GetDisplayName()
		{
			return "Plasmatic Frigate";
		}
	}
}
