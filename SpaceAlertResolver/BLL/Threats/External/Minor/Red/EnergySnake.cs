using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Players;
using BLL.Tracks;

namespace BLL.Threats.External.Minor.Red
{
    public class EnergySnake : MinorRedExternalThreat
    {
        private int healthAtStartOfTurn;
        private bool TookDamageThisTurn => healthAtStartOfTurn > RemainingHealth;

        internal EnergySnake()
            : base(3, 6, 4)
        {
            healthAtStartOfTurn = RemainingHealth;
        }

        public override void PlaceOnTrack(Track track, int trackPosition)
        {
            base.PlaceOnTrack(track, trackPosition);
            ThreatController.DamageResolutionEnding += OnDamageResolutionEnding;
        }

        protected override void PerformXAction(int currentTurn)
        {
            var damageShielded = Attack(1);
            Repair(damageShielded);
        }

        protected override void PerformYAction(int currentTurn)
        {
            var damageShielded = Attack(2);
            Repair(damageShielded);
        }

        protected override void PerformZAction(int currentTurn)
        {
            Attack(1 + SittingDuck.GetEnergyInReactor(CurrentZone));
        }
        private void OnDamageResolutionEnding(object sender, EventArgs args)
        {
            if (TookDamageThisTurn)
                Speed -= 2;
        }

        protected override void OnTurnEnded(object sender, EventArgs args)
        {
            base.OnTurnEnded(sender, args);
            if (TookDamageThisTurn)
                Speed += 2;
            healthAtStartOfTurn = RemainingHealth;
        }

        public override void TakeDamage(IList<PlayerDamage> damages)
        {
            var hitByPulse = damages.Any(damage => damage.PlayerDamageType == PlayerDamageType.Pulse);
            if (hitByPulse)
            {
                var oldShields = Shields;
                Shields = 0;
                base.TakeDamage(damages);
                Shields = oldShields;
            }
            else
                base.TakeDamage(damages);
        }

        protected override void OnThreatTerminated()
        {
            base.OnThreatTerminated();
            ThreatController.DamageResolutionEnding -= OnDamageResolutionEnding;
        }

        public override string Id { get; } = "E3-109";
        public override string DisplayName { get; } = "Energy Snake";
        public override string FileName { get; } = "EnergySnake";
    }
}
