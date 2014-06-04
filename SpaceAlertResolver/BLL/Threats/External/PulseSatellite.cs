using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class PulseSatellite : SeriousWhiteExternalThreat
	{
		public PulseSatellite(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(2, 4, 3, timeAppears, currentZone, sittingDuck)
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
			AttackAllZones(3);
		}

		public static string GetDisplayName()
		{
			return "Pulse Satellite";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.Range != 3 && base.CanBeTargetedBy(damage);
		}
	}
}
