using System.Collections.Generic;
using System.Linq;

namespace BLL.Threats.External.Minor.White
{
	public class SpinningSaucer : MinorWhiteExternalThreat
	{
		private bool hitByRocket;

		public SpinningSaucer()
			: base(4, 3, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			if (!hitByRocket)
				AttackCurrentZone(5);
		}

		public override string Id { get; } = "E1-102";
		public override string DisplayName { get; } = "Spinning Saucer";
		public override string FileName { get; } = "SpinningSaucer";

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			if (damages.Any(damage => damage.PlayerDamageType == PlayerDamageType.Rocket))
			{
				hitByRocket = true;
				DebuffCount++;
			}
			base.TakeDamage(damages);
		}
	}
}
