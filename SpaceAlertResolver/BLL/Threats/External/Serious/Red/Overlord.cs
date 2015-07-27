using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Red
{
	public class Overlord : SeriousRedExternalThreat
	{
		private readonly ExternalThreat threatToCallIn;
		private bool calledInThreat;

		public Overlord(ExternalThreat threatToCallIn)
			: base(5, 14, 2)
		{
			this.threatToCallIn = threatToCallIn;
		}

		protected override void PerformXAction(int currentTurn)
		{
			shields = 4;
			CallInExternalThreat(currentTurn);
		}

		public override int GetPointsForDefeating()
		{
			return 8 + (calledInThreat ? 0 : threatToCallIn.GetPointsForDefeating());
		}

		protected override int GetPointsForSurviving()
		{
			return 4;
		}

		private void CallInExternalThreat(int currentTurn)
		{
			ThreatController.AddExternalThreat(SittingDuck, threatToCallIn, 1000 + currentTurn, CurrentZone);
			calledInThreat = true;
		}

		protected override void PerformYAction(int currentTurn)
		{
			foreach (var threat in ThreatController.DamageableExternalThreats)
				threat.Repair(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			throw new LoseException(this);
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.AffectedDistances.Contains(DistanceToShip);
		}

		public static string GetDisplayName()
		{
			return "Overlord";
		}

		public static string GetId()
		{
			return "SE3-105";
		}
	}
}
