using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class Scout : MinorYellowExternalThreat
	{
		public Scout(int timeAppears, ZoneLocation currentZone, SittingDuck sittingDuck)
			: base(1, 3, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			sittingDuck.CurrentThreatBuffs[this] = ExternalThreatBuff.BonusAttack;
		}

		public override void PerformYAction()
		{
			//TODO: Other threats advance 1 square
			throw new NotImplementedException();
		}

		public override void PerformZAction()
		{
			Attack(3, ThreatDamageType.IgnoresShields);
		}

		protected override void OnDestroyed()
		{
			sittingDuck.CurrentThreatBuffs.Remove(this);
			base.OnDestroyed();
		}
	}
}
