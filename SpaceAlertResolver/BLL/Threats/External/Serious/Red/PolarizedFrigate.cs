using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Red
{
	public class PolarizedFrigate : SeriousRedExternalThreat
	{
		public PolarizedFrigate()
			: base(2, 5, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(3);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(5);
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			//TODO: Code cleanup: Do this better, since it will copy the range and type of a single laser damage atm
			var modifiedDamages = damages.ToList();
			var laserDamages = modifiedDamages.Where(damage => damage.PlayerDamageType == PlayerDamageType.LightLaser || damage.PlayerDamageType == PlayerDamageType.HeavyLaser).ToList();
			if (laserDamages.Any())
			{
				var laserDamageTotal = (int)Math.Ceiling(laserDamages.Sum(laserDamage => laserDamage.Amount) / 2.0);
				modifiedDamages = modifiedDamages.Except(laserDamages).ToList();
				var halfLaserDamage = new PlayerDamage(laserDamages.First()) { PerformingPlayer = null, Amount = laserDamageTotal };
				modifiedDamages.Add(halfLaserDamage);
			}
			base.TakeDamage(modifiedDamages);
		}

		public static string GetDisplayName()
		{
			return "Polarized Frigate";
		}
	}
}
