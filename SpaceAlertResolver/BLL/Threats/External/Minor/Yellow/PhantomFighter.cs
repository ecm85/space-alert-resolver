using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.Yellow
{
	public class PhantomFighter : MinorYellowExternalThreat
	{
		private bool phantomMode = true;

		public PhantomFighter()
			: base(3, 3, 3)
		{
		}

		public override void PerformXAction()
		{
			phantomMode = false;
		}

		public override void PerformYAction()
		{
			Attack(2);
		}

		public override void PerformZAction()
		{
			Attack(3);
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			base.TakeDamage(damages.Where(damage => damage.PlayerDamageType != PlayerDamageType.Rocket).ToList());
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return !phantomMode && base.CanBeTargetedBy(damage);
		}

		public static string GetDisplayName()
		{
			return "Phantom Fighter";
		}
	}
}
