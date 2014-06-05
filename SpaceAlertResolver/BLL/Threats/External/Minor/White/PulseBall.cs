using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.White
{
	public class PulseBall : MinorWhiteExternalThreat
	{
		public PulseBall(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(1, 5, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			AttackAllZones(1);
		}

		public override void PerformYAction()
		{
			AttackAllZones(2);
		}

		public override void PerformZAction()
		{
			AttackAllZones(2);
		}

		public static string GetDisplayName()
		{
			return "Pulse Ball";
		}
	}
}
