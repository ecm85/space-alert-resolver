using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.External.Serious.Red
{
	public class TransmitterSatellite : SeriousRedExternalThreat
	{
		public TransmitterSatellite()
			: base(2, 5, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			//TODO: Calls in internal threat
			//TODO: Points
			//Killed before x: worth 8 + internal threat points
			//Killed after x: worth 8, internal threat worth normal points
			//Hits Z: worth 4, internal threat worth normal points
			throw new NotImplementedException();
		}

		protected override void PerformYAction(int currentTurn)
		{
			ThreatController.MoveInternalThreats(currentTurn, 1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.KnockOutCaptain();
			SittingDuck.ShiftPlayers(EnumFactory.All<StationLocation>(), currentTurn + 1);
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return DistanceToShip != 3 && base.CanBeTargetedBy(damage);
		}

		public static string GetDisplayName()
		{
			return "Transmitter Satellite";
		}
	}
}
