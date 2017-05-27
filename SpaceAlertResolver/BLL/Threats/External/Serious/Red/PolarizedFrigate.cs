using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Players;

namespace BLL.Threats.External.Serious.Red
{
	public class PolarizedFrigate : SeriousRedExternalThreat
	{
		internal PolarizedFrigate()
			: base(2, 5, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(3);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(5);
		}

		public override string Id { get; } = "SE3-109";
		public override string DisplayName { get; } = "Polarized Frigate";
		public override string FileName { get; } = "PolarizedFrigate";

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			var laserDamages = damages.Where(damage => damage.PlayerDamageType == PlayerDamageType.LightLaser || damage.PlayerDamageType == PlayerDamageType.HeavyLaser).ToList();
			var otherDamages = damages.Except(laserDamages).ToList();
			var laserDamageSum = (int)Math.Ceiling(laserDamages.Sum(laserDamage => laserDamage.Amount) / 2.0);
			var otherDamageSum = otherDamages.Sum(damage => damage.Amount);
			TakeDamage(laserDamageSum + otherDamageSum, null);
		}
	}
}
