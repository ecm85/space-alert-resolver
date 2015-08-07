using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.External.Serious.Red
{
	public class Leaper : SeriousRedExternalThreat
	{
		public Leaper()
			: base(2, 7, 2)
		{
		}

		private void Jump(ZoneLocation newZone)
		{
			var newTrack = ThreatController.ExternalTracks[newZone];
			if (newTrack.GetStartingPosition() >= Position)
				Track = newTrack;
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(1);
			Jump(CurrentZone.BluewardZoneLocationWithWrapping());
			AttackCurrentZone(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(2);
			Jump(CurrentZone.RedwardZoneLocationWithWrapping());
			AttackCurrentZone(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(4);
			Jump(CurrentZone.BluewardZoneLocationWithWrapping());
			AttackCurrentZone(1);
		}
	}
}
