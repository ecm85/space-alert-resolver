using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.Yellow
{
	public class Marauder : MinorYellowExternalThreat
	{
		public Marauder(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(1, 6, 3, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			sittingDuck.AddExternalThreatBuff(ExternalThreatBuff.BonusShield, this);
		}

		public override void PerformYAction()
		{
			sittingDuck.DrainShields(EnumFactory.All<ZoneLocation>(), 1);
		}

		public override void PerformZAction()
		{
			Attack(4);
		}

		protected override void OnDestroyed()
		{
			sittingDuck.RemoveExternalThreatBuffForSource(this);
			base.OnDestroyed();
		}

		public static string GetDisplayName()
		{
			return "Marauder";
		}
	}
}
