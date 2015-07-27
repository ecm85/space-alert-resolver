using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Red
{
	public class MegashieldManOfWar : SeriousRedExternalThreat
	{
		public MegashieldManOfWar()
			: base(5, 7, 1)
		{
		}

		public static string GetDisplayName()
		{
			return "Megashield Man-Of-War";
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(2);
			Speed++;
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(3);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(5);
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			base.TakeDamage(damages);
			if (damages.Any() && shields > 0)
				shields--;
		}

		public static string GetId()
		{
			return "SE3-102";
		}
	}
}
