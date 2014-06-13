using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.White
{
	public class CryoshieldFrigate : SeriousWhiteExternalThreat
	{
		private bool cryoshieldUp = true;

		public CryoshieldFrigate()
			: base(1, 7, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(3);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(4);
		}

		public static string GetDisplayName()
		{
			return "Cryoshield Frigate";
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			if (cryoshieldUp && damages.Any())
				cryoshieldUp = false;
			else
				base.TakeDamage(damages);
		}
	}
}
