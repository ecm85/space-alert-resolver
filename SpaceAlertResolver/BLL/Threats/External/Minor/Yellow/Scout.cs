using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.Yellow
{
	public class Scout : MinorYellowExternalThreat
	{
		public Scout()
			: base(1, 3, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			ThreatController.AddExternalThreatBuff(ExternalThreatBuff.BonusAttack, this);
		}

		protected override void PerformYAction(int currentTurn)
		{
			ThreatController.MoveOtherExternalThreats(currentTurn, 1, this);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(2, ThreatDamageType.IgnoresShields);
		}

		protected override void OnHealthReducedToZero()
		{
			ThreatController.RemoveExternalThreatBuffForSource(this);
			base.OnHealthReducedToZero();
		}

		public static string GetDisplayName()
		{
			return "Scout";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.HeavyLaser && base.CanBeTargetedBy(damage);
		}
	}
}
