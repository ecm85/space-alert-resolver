using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Yellow
{
	public class Nemesis : SeriousYellowExternalThreat
	{
		private int healthAtStartOfTurn;

		public Nemesis(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(1, 9, 3, timeAppears, currentZone, sittingDuck)
		{
			healthAtStartOfTurn = RemainingHealth;
		}

		public override void PeformXAction()
		{
			Attack(1);
			RemainingHealth--;
			CheckForDestroyed();
		}

		public override void PerformYAction()
		{
			Attack(2);
			RemainingHealth -= 2;
			CheckForDestroyed();
		}

		public override void PerformZAction()
		{
			throw new LoseException(this);
		}

		public override void PerformEndOfComputeDamage()
		{
			if (healthAtStartOfTurn > RemainingHealth)
				AttackAllZones(1);
			base.PerformEndOfComputeDamage();
		}

		public static string GetDisplayName()
		{
			return "Nemesis";
		}

		public override void PerformEndOfTurn()
		{
			base.PerformEndOfTurn();
			healthAtStartOfTurn = RemainingHealth;
		}
	}
}
