using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.White
{
	public class StealthFighter : MinorWhiteExternalThreat
	{
		private bool stealthed = true;

		public StealthFighter()
			: base(2, 4, 3)
		{
		}

		public override void PerformXAction(int currentTurn)
		{
			stealthed = false;
		}

		public override void PerformYAction(int currentTurn)
		{
			Attack(2);
		}

		public override void PerformZAction(int currentTurn)
		{
			Attack(2);
		}

		public static string GetDisplayName()
		{
			return "Stealth Fighter";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return !stealthed && base.CanBeTargetedBy(damage);
		}
	}
}
