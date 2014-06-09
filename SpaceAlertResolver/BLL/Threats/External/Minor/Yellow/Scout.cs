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

		public override void PerformXAction(int currentTurn)
		{
			SittingDuck.AddExternalThreatBuff(ExternalThreatBuff.BonusAttack, this);
		}

		public override void PerformYAction(int currentTurn)
		{
			ThreatController.MoveExternalThreatsExcept(new [] {this}, 1, currentTurn);
		}

		public override void PerformZAction(int currentTurn)
		{
			Attack(2, ThreatDamageType.IgnoresShields);
		}

		protected override void OnHealthReducedToZero()
		{
			SittingDuck.RemoveExternalThreatBuffForSource(this);
			base.OnHealthReducedToZero();
		}

		public static string GetDisplayName()
		{
			return "Scout";
		}
	}
}
