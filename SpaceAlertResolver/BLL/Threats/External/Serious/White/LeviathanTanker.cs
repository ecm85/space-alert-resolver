using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.White
{
	public class LeviathanTanker : SeriousWhiteExternalThreat
	{
		public LeviathanTanker()
			: base(3, 8, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(2);
			Repair(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(2);
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnHealthReducedToZero();
			foreach (var threat in ThreatController.DamageableExternalThreats)
				threat.TakeIrreducibleDamage(1);
		}

		public static string GetDisplayName()
		{
			return "Leviathan Tanker";
		}
	}
}
