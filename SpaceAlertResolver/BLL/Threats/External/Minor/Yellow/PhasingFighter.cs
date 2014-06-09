using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.Yellow
{
	public class PhasingFighter : MinorYellowExternalThreat
	{
		private bool isPhased;
		private bool wasPhasedAtStartOfTurn;

		public PhasingFighter()
			: base(2, 4, 3)
		{
		}

		public override void PerformXAction()
		{
			Attack(1);
		}

		public override void PerformYAction()
		{
			Attack(wasPhasedAtStartOfTurn ? 1 : 2);
		}

		public override void PerformZAction()
		{
			Attack(wasPhasedAtStartOfTurn ? 2 : 3);
		}

		protected override void BeforeMove()
		{
			base.BeforeMove();
			isPhased = false;
		}

		protected override void AfterMove()
		{
			base.AfterMove();
			isPhased = !wasPhasedAtStartOfTurn;
		}

		public override void PerformEndOfTurn()
		{
			wasPhasedAtStartOfTurn = isPhased;
			base.PerformEndOfTurn();
		}

		public override void TakeIrreducibleDamage(int amount)
		{
			if (!isPhased)
				base.TakeIrreducibleDamage(amount);
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return !isPhased && base.CanBeTargetedBy(damage);
		}

		public static string GetDisplayName()
		{
			return "Phasing Fighter";
		}
	}
}
