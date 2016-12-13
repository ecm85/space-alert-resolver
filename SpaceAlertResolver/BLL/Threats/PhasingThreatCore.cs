
using System;

namespace BLL.Threats
{
	public class PhasingThreatCore
	{
		private bool isPhasedOut;
		private readonly Threat threat;

		public PhasingThreatCore(Threat threat)
		{
			this.threat = threat;
			threat.Moving += PhaseIn;
			threat.Moved += TogglePhasing;
			threat.TurnEnded += RecordPhasingStatus;
		}

		private void RecordPhasingStatus(object sender, EventArgs args)
		{
			WasPhasedOutAtStartOfTurn = isPhasedOut;
		}

		public bool IsDamageable => !isPhasedOut;

		public bool WasPhasedOutAtStartOfTurn { get; private set; }

		private void PhaseIn(object sender, EventArgs args)
		{
			isPhasedOut = false;
			WasPhasedOutAtStartOfTurn = false;
		}

		private void TogglePhasing(object sender, EventArgs args)
		{
			isPhasedOut = !WasPhasedOutAtStartOfTurn;
		}

		public void ThreatTerminated()
		{
			threat.Moving -= PhaseIn;
			threat.Moved -= TogglePhasing;
			threat.TurnEnded -= RecordPhasingStatus;
		}
	}
}
