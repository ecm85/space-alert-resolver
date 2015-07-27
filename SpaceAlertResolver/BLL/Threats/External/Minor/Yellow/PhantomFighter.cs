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

		protected override void PerformXAction(int currentTurn)
		{
			phantomMode = false;
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(2);
		}

		protected override void PerformZAction(int currentTurn)
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

		public static string GetId()
		{
			return "E2-03";
		}
	}
}
