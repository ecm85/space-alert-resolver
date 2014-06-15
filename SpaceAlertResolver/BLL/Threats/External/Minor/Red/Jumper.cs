using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			Attack(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(1);
			Jump(JumpDestination);
			Attack(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(3);
			Jump(JumpDestination);
			Attack(1);
		}

		protected abstract ZoneLocation JumpDestination { get; }

		private void Jump(ZoneLocation newZone)
		{
			var newTrack = ThreatController.ExternalTracks[newZone];
			if (newTrack.GetStartingPosition() < Position)
				return;
			Track = newTrack;
		}
	}
}
