using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.White
{
	public class ManOfWar : SeriousWhiteExternalThreat
	{
		public ManOfWar(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(2, 9, 1, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			Attack(2);
			Speed++;
		}

		public override void PerformYAction()
		{
			Attack(3);
			shields++;
		}

		public override void PerformZAction()
		{
			Attack(3);
		}

		public static string GetDisplayName()
		{
			return "Man-Of-War";
		}
	}
}
