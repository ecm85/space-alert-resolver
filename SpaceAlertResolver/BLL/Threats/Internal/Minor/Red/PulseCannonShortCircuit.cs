using System;
using BLL.Players;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Minor.Red
{
    public class PulseCannonShortCircuit : MinorRedInternalThreat
    {
        internal PulseCannonShortCircuit()
            : base(2, 2, StationLocation.LowerWhite, PlayerActionType.Alpha, 1)
        {
        }

        public override void PlaceOnTrack(Track track, int trackPosition)
        {
            base.PlaceOnTrack(track, trackPosition);
            SittingDuck.CentralLaserCannonFired += HandleCentralLaserCannonFired;
        }

        private void HandleCentralLaserCannonFired(object sender, EventArgs args)
        {
            AttackAllZones(1);
        }
        protected override void PerformXAction(int currentTurn)
        {
            var energyDrained = SittingDuck.DrainReactors(new [] {CurrentZone}, 1);
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
            AttackAllZones(amount, ThreatDamageType.Standard);
        }

        protected override void OnThreatTerminated()
        {
            base.OnThreatTerminated();
            SittingDuck.CentralLaserCannonFired -= HandleCentralLaserCannonFired;
        }

        public override string Id { get; } = "I3-102";
        public override string DisplayName { get; } = "Pulse Cannon Short Circuit";
        public override string FileName { get; } = "PulseCannonShortCircuit";
    }
}
