using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class Marauder : MinorYellowExternalThreat
	{
		public Marauder(int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(1, 6, 3, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			sittingDuck.CurrentThreatBuffs[this] = ExternalThreatBuff.BonusShield;
		}

		public override void PerformYAction()
		{
			var allShields = sittingDuck.Zones.Select(zone => zone.UpperStation.EnergyContainer);
			foreach (var shield in allShields)
				shield.Energy -= 1;
		}

		public override void PerformZAction()
		{
			Attack(4);
		}

		protected override void OnDestroyed()
		{
			sittingDuck.CurrentThreatBuffs.Remove(this);
			base.OnDestroyed();
		}
	}
}
