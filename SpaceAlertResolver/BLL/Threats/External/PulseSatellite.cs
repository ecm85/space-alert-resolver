using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class PulseSatellite : SeriousWhiteExternalThreat
	{
		public PulseSatellite(int shields, int health, int speed, int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(shields, health, speed, timeAppears, currentZone, sittingDuck)
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

		public override string GetDisplayName()
		{
			return "Pulse Satellite";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.Range != 3 && base.CanBeTargetedBy(damage);
		}
	}
}
