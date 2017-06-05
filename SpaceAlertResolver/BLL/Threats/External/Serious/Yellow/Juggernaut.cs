using System.Collections.Generic;
using System.Linq;
using BLL.Common;
using BLL.Players;

namespace BLL.Threats.External.Serious.Yellow
{
	public class Juggernaut : SeriousYellowExternalThreat
	{
		internal Juggernaut()
			: base(3, 10, 1)
		{
		}
		protected override void PerformXAction(int currentTurn)
		{
			Speed += 2;
			Attack(2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Speed += 2;
			Attack(3);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(7);
		}

		public override string Id { get; } = "SE2-02";
		public override string DisplayName { get; } = "Juggernaut";
		public override string FileName { get; } = "Juggernaut";

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			base.TakeDamage(damages);
			if (damages.Any(damage => damage.PlayerDamageType == PlayerDamageType.Rocket))
				Shields++;
		}

		public override bool IsPriorityTargetFor(PlayerDamage damage)
		{
			Check.ArgumentIsNotNull(damage, "damage");
			return damage.PlayerDamageType == PlayerDamageType.Rocket;
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			Check.ArgumentIsNotNull(damage, "damage");
			return base.CanBeTargetedBy(damage) || damage.PlayerDamageType == PlayerDamageType.Rocket;
		}
	}
}
