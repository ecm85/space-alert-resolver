using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.White
{
	public class InterstellarOctopus : SeriousWhiteExternalThreat
	{
		public InterstellarOctopus()
			: base(1, 8, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			if (IsDamaged)
				AttackAllZones(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			if (IsDamaged)
				AttackAllZones(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(RemainingHealth * 2);
		}

		public static string GetDisplayName()
		{
			return "Interstellar Octopus";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}

		public static string GetId()
		{
			return "SE1-06";
		}
	}
}
