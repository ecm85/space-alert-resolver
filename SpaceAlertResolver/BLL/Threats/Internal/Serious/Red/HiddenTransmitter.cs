using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats.External;

namespace BLL.Threats.Internal.Serious.Red
{
	public abstract class HiddenTransmitter : SeriousRedInternalThreat, IThreatWithBonusThreat<ExternalThreat>
	{
		private ExternalThreat BonusThreat { get; set; }
		private bool calledInThreat;
		protected HiddenTransmitter(StationLocation stationLocation)
			: base(3, 2, stationLocation, PlayerActionType.C, 1)
		{
		}

		public void SetBonusThreat(ExternalThreat bonusThreat)
		{
			BonusThreat = bonusThreat;
		}

		public override bool NeedsBonusExternalThreat { get { return true; } }

		protected override void PerformXAction(int currentTurn)
		{
			TotalInaccessibility = 0;
			CallInExternalThreat(currentTurn);
		}

		private void CallInExternalThreat(int currentTurn)
		{
			ThreatController.AddExternalThreat(BonusThreat, 1000 + currentTurn, CurrentZone);
			calledInThreat = true;
		}

		public override int GetPointsForDefeating()
		{
			return 8 + (calledInThreat ? 0 : BonusThreat.GetPointsForDefeating());
		}

		protected override int GetPointsForSurviving()
		{
			return 4;
		}

		protected override void PerformYAction(int currentTurn)
		{
			ThreatController.MoveExternalThreatsInZone(currentTurn, 2, CurrentZone);
		}
	}
}
