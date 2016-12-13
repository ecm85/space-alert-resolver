using BLL.ShipComponents;

namespace BLL.Threats.External.Minor.Red
{
	public abstract class Jumper : MinorRedExternalThreat
	{
		protected Jumper()
			: base(1, 6, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Jump(JumpDestination);
			AttackCurrentZone(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(1);
			Jump(JumpDestination);
			AttackCurrentZone(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(3);
			Jump(JumpDestination);
			AttackCurrentZone(1);
		}

		protected abstract ZoneLocation JumpDestination { get; }

		private void Jump(ZoneLocation newZone)
		{
			var newTrack = ThreatController.ExternalTracks[newZone];
			if (newTrack.GetStartingPosition() >= Position)
				Track = newTrack;
		}
	}
}
