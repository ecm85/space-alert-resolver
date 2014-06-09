using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.White
{
	public class Asteroid : SeriousWhiteExternalThreat
	{
		private int breakpointsCrossed;

		public Asteroid()
			: base(0, 9, 3)
		{
		}

		public override void PerformXAction()
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
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnHealthReducedToZero();
			Attack(2 * breakpointsCrossed);
		}

		public static string GetDisplayName()
		{
			return "Asteroid";
		}
	}
}
