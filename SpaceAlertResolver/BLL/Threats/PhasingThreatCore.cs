
using System;

namespace BLL.Threats
{
	public class PhasingThreatCore
	{
		private readonly Threat threat;

		private bool IsPhasedOut
		{
			get { return threat.GetThreatStatus(ThreatStatus.PhasedOut); } 
			set { threat.SetThreatStatus(ThreatStatus.PhasedOut, value); }
		}

		public PhasingThreatCore(Threat threat)
		{
			this.threat = threat;
			threat.Moving += PhaseIn;
			threat.Moved += TogglePhasing;
			threat.TurnEnded += RecordPhasingStatus;
		}

		private void RecordPhasingStatus(object sender, EventArgs args)
		{
			WasPhasedOutAtStartOfTurn = IsPhasedOut;
		}

		public bool IsDamageable => !IsPhasedOut;

		public bool WasPhasedOutAtStartOfTurn { get; private set; }

		private void PhaseIn(object sender, EventArgs args)
		{
			IsPhasedOut = false;
			WasPhasedOutAtStartOfTurn = false;
		}

		private void TogglePhasing(object sender, EventArgs args)
		{
			IsPhasedOut = !WasPhasedOutAtStartOfTurn;
		}

		public void ThreatTerminated()
		{
			threat.Moving -= PhaseIn;
			threat.Moved -= TogglePhasing;
			threat.TurnEnded -= RecordPhasingStatus;
		}
	}
}
