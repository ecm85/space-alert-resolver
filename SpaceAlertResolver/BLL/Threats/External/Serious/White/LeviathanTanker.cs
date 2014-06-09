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

		public override void PerformXAction()
		{
			Attack(2);
		}

		public override void PerformYAction()
		{
			Attack(2);
			Repair(2);
		}

		public override void PerformZAction()
		{
			Attack(2);
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnHealthReducedToZero();
			foreach (var threat in ThreatController.ExternalThreats)
				threat.TakeIrreducibleDamage(1);
		}

		public static string GetDisplayName()
		{
			return "Leviathan Tanker";
		}
	}
}
