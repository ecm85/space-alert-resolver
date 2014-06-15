using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.External.Serious.White
{
	public class DimensionSpider : SeriousWhiteExternalThreat
	{
		public DimensionSpider()
			: base(0, 13, 1)
		{
		}

		public override void Initialize(ISittingDuck sittingDuck, ThreatController threatController, int timeAppears, ZoneLocation currentZone)
		{
			base.Initialize(sittingDuck, threatController, timeAppears, currentZone);
			ThreatController.JumpingToHyperspace += OnJumpingToHyperspace;
		}

		protected override void PerformXAction(int currentTurn)
		{
			shields = 1;
		}

		protected override void PerformYAction(int currentTurn)
		{
			shields++;
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackAllZones(4);
		}

		private void OnJumpingToHyperspace()
		{
			if (HasBeenPlaced)
				PerformZAction(-1);
		}

		public static string GetDisplayName()
		{
			return "Dimension Spider";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}

		protected override void OnThreatTerminated()
		{
			base.OnThreatTerminated();
			ThreatController.JumpingToHyperspace -= OnJumpingToHyperspace;
		}
	}
}
