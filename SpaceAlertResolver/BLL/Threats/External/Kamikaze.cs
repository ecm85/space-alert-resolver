using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class Kamikaze : MinorYellowExternalThreat
	{
		public Kamikaze(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(2, 5, 4, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			Speed++;
			Shields = 1;
		}

		public override void PerformYAction()
		{
			Speed++;
			Shields = 0;
		}

		public override void PerformZAction()
		{
			Attack(6);
		}

		public static string GetDisplayName()
		{
			return "Kamikaze";
		}
	}
}
