using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.Red
{
	public class PlasmaticNeedleship : MinorRedExternalThreat
	{
		public PlasmaticNeedleship()
			: base(1, 3, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(1, ThreatDamageType.Plasmatic);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(2, ThreatDamageType.Plasmatic);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(4, ThreatDamageType.Plasmatic);
		}
		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.HeavyLaser && base.CanBeTargetedBy(damage);
		}
	}
}
