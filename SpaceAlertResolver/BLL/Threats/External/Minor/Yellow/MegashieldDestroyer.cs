using System.Collections.Generic;
using System.Linq;
using BLL.Players;

namespace BLL.Threats.External.Minor.Yellow
{
	public class MegashieldDestroyer : MinorYellowExternalThreat
	{
		internal MegashieldDestroyer()
			: base(4, 3, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(1, ThreatDamageType.DoubleDamageThroughShields);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(1, ThreatDamageType.DoubleDamageThroughShields);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(3, ThreatDamageType.DoubleDamageThroughShields);
		}

		public override string Id { get; } = "E2-103";
		public override string DisplayName { get; } = "Megashield Destroyer";
		public override string FileName { get; } = "MegashieldDestroyer";

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			base.TakeDamage(damages);
			if (damages.Any() && Shields > 0)
				Shields--;
		}
	}
}
