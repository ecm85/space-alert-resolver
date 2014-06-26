using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats.External;

namespace BLL.Threats.Internal.Serious.Red
{
	public abstract class HiddenTransmitter : SeriousRedInternalThreat
	{
		private readonly ExternalThreat threatToCallIn;
		private bool calledInThreat;
		protected HiddenTransmitter(StationLocation stationLocation, ExternalThreat threatToCallIn)
			: base(3, 2, stationLocation, PlayerActionType.C, 1)
		{
			this.threatToCallIn = threatToCallIn;
		}

		protected override void PerformXAction(int currentTurn)
		{
			totalInaccessibility = 0;
			CallInExternalThreat(currentTurn);
		}

		private void CallInExternalThreat(int currentTurn)
		{
			ThreatController.AddExternalThreat(SittingDuck, threatToCallIn, 1000 + currentTurn, CurrentZone);
			calledInThreat = true;
		}

		public override int GetPointsForDefeating()
		{
			return 8 + (calledInThreat ? 0 : threatToCallIn.GetPointsForDefeating());
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
