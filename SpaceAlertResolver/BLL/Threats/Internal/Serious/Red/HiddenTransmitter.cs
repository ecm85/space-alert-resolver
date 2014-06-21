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
			: base(3, 2, stationLocation, PlayerAction.C, 1)
		{
			this.threatToCallIn = threatToCallIn;
		}

		protected override void PerformXAction(int currentTurn)
		{
			totalInaccessibility = 0;
			CallInExternalThreat(currentTurn);
			throw new NotImplementedException();
		}

		private void CallInExternalThreat(int currentTurn)
		{
			threatToCallIn.Initialize(SittingDuck, ThreatController, 1000 + currentTurn, CurrentZone);
			threatToCallIn.PlaceOnTrack(ThreatController.ExternalTracks[CurrentZone]);
			ThreatController.AddExternalThreat(threatToCallIn);
			calledInThreat = true;
		}

		public override int GetPointsForDefeating()
		{
			return 8 + (calledInThreat ? 0 : threatToCallIn.GetPointsForDefeating());
		}

		public override int GetPointsForSurviving()
		{
			return 4;
		}

		protected override void PerformYAction(int currentTurn)
		{
			//TODO: Move all threats in current zone
			throw new NotImplementedException();
		}
	}
}
