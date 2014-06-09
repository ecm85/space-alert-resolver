using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.Yellow
{
	public class Marauder : MinorYellowExternalThreat
	{
		public Marauder()
			: base(1, 6, 3)
		{
		}

		public override void PerformXAction()
		{
			SittingDuck.AddExternalThreatBuff(ExternalThreatBuff.BonusShield, this);
		}

		public override void PerformYAction()
		{
			SittingDuck.DrainShields(EnumFactory.All<ZoneLocation>(), 1);
		}

		public override void PerformZAction()
		{
			Attack(4);
		}

		protected override void OnHealthReducedToZero()
		{
			SittingDuck.RemoveExternalThreatBuffForSource(this);
			base.OnHealthReducedToZero();
		}

		public static string GetDisplayName()
		{
			return "Marauder";
		}
	}
}
