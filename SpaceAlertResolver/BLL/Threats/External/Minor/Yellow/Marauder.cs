using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.External.Minor.Yellow
{
	public class Marauder : MinorYellowExternalThreat
	{
		public Marauder()
			: base(1, 6, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			ThreatController.AddExternalThreatEffect(ExternalThreatEffect.BonusShield, this);
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.DrainShields(EnumFactory.All<ZoneLocation>(), 1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(4);
		}

		protected override void OnHealthReducedToZero()
		{
			ThreatController.RemoveExternalThreatEffectForSource(this);
			base.OnHealthReducedToZero();
		}
	}
}
