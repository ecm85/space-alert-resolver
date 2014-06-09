using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Yellow
{
	public class PhasingFrigate : SeriousYellowExternalThreat
	{
		private bool isPhased;
		private bool wasPhasedAtStartOfTurn;

		public PhasingFrigate()
			: base(2, 7, 2)
		{
		}

		public override void PerformXAction(int currentTurn)
		{
			Attack(wasPhasedAtStartOfTurn ? 1 : 2);
		}

		public override void PerformYAction(int currentTurn)
		{
			Attack(wasPhasedAtStartOfTurn ? 2 : 3);
		}

		public override void PerformZAction(int currentTurn)
		{
			Attack(wasPhasedAtStartOfTurn ? 3 : 4);
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
			return "Phasing Frigate";
		}
	}
}
