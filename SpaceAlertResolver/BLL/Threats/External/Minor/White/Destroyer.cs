using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.White
{
	public class Destroyer : MinorWhiteExternalThreat
	{
		public Destroyer()
			: base(2, 5, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(1, ThreatDamageType.DoubleDamageThroughShields);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(2, ThreatDamageType.DoubleDamageThroughShields);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(2, ThreatDamageType.DoubleDamageThroughShields);
		}

		public static string GetDisplayName()
		{
			return "Destroyer";
		}
	}
}
