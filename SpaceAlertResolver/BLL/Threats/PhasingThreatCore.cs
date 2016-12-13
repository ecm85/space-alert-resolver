
namespace BLL.Threats
{
	public class PhasingThreatCore
	{
		private bool isPhasedOut;
		private readonly Threat threat;

		public PhasingThreatCore(Threat threat)
		{
			this.threat = threat;
			threat.MovingEventHandler += PhaseIn;
			threat.MovedEventHandler += TogglePhasing;
			threat.EndOfTurnEventHandler += RecordPhasingStatus;
		}

		private void RecordPhasingStatus()
		{
			WasPhasedOutAtStartOfTurn = isPhasedOut;
		}

		public bool IsDamageable => !isPhasedOut;

		public bool WasPhasedOutAtStartOfTurn { get; private set; }

		private void PhaseIn()
		{
			isPhasedOut = false;
			WasPhasedOutAtStartOfTurn = false;
		}

		private void TogglePhasing()
		{
			isPhasedOut = !WasPhasedOutAtStartOfTurn;
		}

		public void ThreatTerminated()
		{
			threat.MovingEventHandler -= PhaseIn;
			threat.MovedEventHandler -= TogglePhasing;
			threat.EndOfTurnEventHandler -= RecordPhasingStatus;
		}
	}
}
