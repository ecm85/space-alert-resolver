using System.Collections.Generic;
using System.Linq;
using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.External.Minor.White
{
	public class EnergyCloud : MinorWhiteExternalThreat
	{
		internal EnergyCloud()
			: base(3, 5, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.DrainShields(EnumFactory.All<ZoneLocation>());
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackOtherTwoZones(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackOtherTwoZones(2);
		}

		public override string Id { get; } = "E1-04";
		public override string DisplayName { get; } = "Energy Cloud";
		public override string FileName { get; } = "EnergyCloud";

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
