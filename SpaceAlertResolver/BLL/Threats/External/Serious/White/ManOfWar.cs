using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.White
{
	public class ManOfWar : SeriousWhiteExternalThreat
	{
		public ManOfWar()
			: base(2, 9, 1)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(2);
			Speed++;
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(3);
			Shields++;
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(3);
		}
	}
}
