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

		protected override void PerformXAction(int currentTurn)
		{
			Attack(1, ThreatDamageType.Plasmatic);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(1, ThreatDamageType.Plasmatic);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(2, ThreatDamageType.Plasmatic);
		}
	}
}
