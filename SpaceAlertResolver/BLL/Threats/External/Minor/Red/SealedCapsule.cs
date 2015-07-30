using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats.Internal;

namespace BLL.Threats.External.Minor.Red
{
	public class SealedCapsule : MinorRedExternalThreat, IThreatWithBonusInternalThreat
	{
		private InternalThreat threatToCallIn;

		public SealedCapsule()
			: base(4, 4, 4)
		{
		}

		public void SetBonusThreat(InternalThreat threatToCallIn)
		{
			this.threatToCallIn = threatToCallIn;
		}

		public override bool NeedsBonusInternalThreat { get { return true; } }

		protected override void PerformXAction(int currentTurn)
		{
			Speed++;
			Shields = 3;
		}

		protected override void PerformYAction(int currentTurn)
		{
			Speed++;
		}

		protected override void PerformZAction(int currentTurn)
		{
			CallInInternalThreat(currentTurn);
			var newthreatGetsExtraSpeed = TotalHealth - RemainingHealth < 2;
			if (newthreatGetsExtraSpeed)
				threatToCallIn.Speed++;
		}

		private void CallInInternalThreat(int currentTurn)
		{
			ThreatController.AddInternalThreat(threatToCallIn, 1000 + currentTurn);
		}

		public override int GetPointsForDefeating()
		{
			return threatToCallIn.GetPointsForDefeating();
		}

		protected override int GetPointsForSurviving()
		{
			return 0;
		}
	}
}
