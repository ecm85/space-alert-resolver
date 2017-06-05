using System.Collections.Generic;
using System.Linq;
using BLL.Players;

namespace BLL.Threats.External.Serious.Red
{
	public class MegashieldManOfWar : SeriousRedExternalThreat
	{
		internal MegashieldManOfWar()
			: base(5, 7, 1)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(2);
			Speed++;
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(3);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(5);
		}

		public override string Id { get; } = "SE3-102";
		public override string DisplayName { get; } = "Megashield Man-Of-War";
		public override string FileName { get; } = "MegashieldManOfWar";

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			base.TakeDamage(damages);
			if (damages.Any() && Shields > 0)
				Shields--;
		}
	}
}
