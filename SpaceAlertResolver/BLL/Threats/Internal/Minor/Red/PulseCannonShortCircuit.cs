using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Minor.Red
{
	public class PulseCannonShortCircuit : MinorRedInternalThreat
	{
		public PulseCannonShortCircuit()
			: base(2, 2, StationLocation.LowerWhite, PlayerActionType.A, 1)
		{
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			SittingDuck.CentralLaserCannonFired += HandleCentralLaerCannonFired;
		}

		private void HandleCentralLaerCannonFired()
		{
			AttackAllZones(1);
		}

		public static string GetDisplayName()
		{
			return "Pulse Cannon Short Circuit";
		}

		protected override void PerformXAction(int currentTurn)
		{
			var energyDrained = SittingDuck.DrainReactor(CurrentZone, 1);
			if (energyDrained == 1)
				AttackAllZones(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			var drainedCapsule = SittingDuck.DestroyFuelCapsule();
			if (drainedCapsule)
				AttackAllZones(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackAllZones(1);
		}

		private void AttackAllZones(int amount)
		{
			SittingDuck.TakeAttack(new ThreatDamage(amount, ThreatDamageType.Standard, EnumFactory.All<ZoneLocation>()));
		}

		protected override void OnThreatTerminated()
		{
			base.OnThreatTerminated();
			SittingDuck.CentralLaserCannonFired -= HandleCentralLaerCannonFired;
		}

		public static string GetId()
		{
			return "I3-102";
		}
	}
}
