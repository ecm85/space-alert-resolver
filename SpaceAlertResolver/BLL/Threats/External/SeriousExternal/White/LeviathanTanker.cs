using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.SeriousExternal.White
{
	public class LeviathanTanker : SeriousWhiteExternalThreat
	{
		public LeviathanTanker(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(3, 8, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
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

		protected override void OnDestroyed()
		{
			base.OnDestroyed();
			foreach (var threat in sittingDuck.CurrentExternalThreats)
			{
				threat.RemainingHealth -= 1;
				threat.CheckForDestroyed();
			}
		}

		public static string GetDisplayName()
		{
			return "Leviathan Tanker";
		}
	}
}
