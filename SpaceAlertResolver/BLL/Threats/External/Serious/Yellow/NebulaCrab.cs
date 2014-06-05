using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Yellow
{
	public class NebulaCrab : SeriousYellowExternalThreat
	{
		public NebulaCrab(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(2, 7, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			shields = 4;
		}

		public override void PerformYAction()
		{
			Speed += 2;
			shields = 2;
		}

		public override void PerformZAction()
		{
			AttackSpecificZones(5, new[] {ZoneLocation.Red, ZoneLocation.Blue});
		}

		public static string GetDisplayName()
		{
			return "Nebula Crab";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
