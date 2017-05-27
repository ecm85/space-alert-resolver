using System.Collections.Generic;
using System.Linq;
using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.External.Serious.White
{
	public class Maelstrom : SeriousWhiteExternalThreat
	{
		internal Maelstrom()
			: base(3, 8, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.DrainShields(EnumFactory.All<ZoneLocation>());
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackOtherTwoZones(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackOtherTwoZones(3);
		}

		public override string Id { get; } = "SE1-07";
		public override string DisplayName { get; } = "Maelstrom";
		public override string FileName { get; } = "Maelstrom";

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			var hitByPulse = damages.Any(damage => damage.PlayerDamageType == PlayerDamageType.Pulse);
			if (hitByPulse)
			{
				var oldShields = Shields;
				Shields = 0;
				base.TakeDamage(damages);
				Shields = oldShields;
			}
			else
				base.TakeDamage(damages);
		}
	}
}
