using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class Asteroid : SeriousWhiteExternalThreat
	{
		private int breakpointsCrossed;

		public Asteroid(int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(0, 9, 3, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			breakpointsCrossed++;
		}

		public override void PerformYAction()
		{
			breakpointsCrossed++;
		}

		public override void PerformZAction()
		{
			Attack(RemainingHealth);
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.DamageType != DamageType.Rocket && base.CanBeTargetedBy(damage);
		}

		protected override void OnDestroyed()
		{
			Attack(2 * breakpointsCrossed);
		}
	}
}
