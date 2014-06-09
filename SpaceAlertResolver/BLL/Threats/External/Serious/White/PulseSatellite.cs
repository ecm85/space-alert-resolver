using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.White
{
	public class PulseSatellite : SeriousWhiteExternalThreat
	{
		public PulseSatellite()
			: base(2, 4, 3)
		{
		}

		public override void PerformXAction()
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
