using System.Collections.Generic;
using System.Linq;
using BLL.Players;

namespace BLL.Threats.External.Minor.Red
{
	public class MegashieldFighter : MinorRedExternalThreat
	{
		internal MegashieldFighter()
			: base(4, 4, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(3);
		}

		public override string Id { get; } = "E3-104";
		public override string DisplayName { get; } = "Megashield Fighter";
		public override string FileName { get; } = "MegashieldFighter";

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			base.TakeDamage(damages);
			if (damages.Any() && Shields > 0)
				Shields--;
		}
	}
}
