using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Common;
using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.External
{
    public abstract class ExternalThreat : Threat
    {
        private ZoneLocation _currentZone;
        public override ZoneLocation CurrentZone => _currentZone;

        public void SetInitialPlacement(int timeAppears, ZoneLocation currentZone)
        {
            TimeAppears = timeAppears;
            _currentZone = currentZone;
        }

        protected void MoveToNewZone(ZoneLocation newZone)
        {
            _currentZone = newZone;
        }

        public int Shields { get; protected set; }
        //public event EventHandler<ThreatDamageEventArgs> TakingDamage = (sender, args) => { };
        //public event EventHandler<ThreatDamageEventArgs> TooKDamage = (sender, args) => { };

        protected int DistanceToShip => Track.DistanceToThreat(Position);

        public override ThreatDamageType StandardDamageType { get; } = ThreatDamageType.Standard;

        public override int? DamageDistanceToSource => DistanceToShip;
        public virtual bool IsDamageable => IsOnTrack;

        protected ExternalThreat(ThreatType threatType, ThreatDifficulty difficulty, int shields, int health, int speed) :
            base(threatType, difficulty, health, speed)
        {
            Shields = shields;
        }

        public void TakeIrreducibleDamage(int amount)
        {
            RemainingHealth -= amount;
            CheckDefeated();
        }

        public virtual void TakeDamage(IList<PlayerDamage> damages)
        {
            TakeDamage(damages, null);
        }

        protected void TakeDamage(IEnumerable<PlayerDamage> damages, int? maxDamageTaken)
        {
            var damageSum = damages.Sum(damage => damage.Amount);
            TakeDamage(damageSum, maxDamageTaken);
        }

        protected void TakeDamage(int damageSum, int? maxDamageTaken)
        {
            var bonusShields = GetThreatStatus(ThreatStatus.BonusShield) ? 1 : 0;
            var damageDealt = damageSum - (Shields + bonusShields);
            if (damageDealt > 0)
                RemainingHealth -= maxDamageTaken.HasValue ? Math.Min(damageDealt, maxDamageTaken.Value) : damageDealt;
            CheckDefeated();
        }

        public virtual bool CanBeTargetedBy(PlayerDamage damage)
        {
            Check.ArgumentIsNotNull(damage, "damage");
            var isInRange = damage.AffectedDistances.Contains(DistanceToShip);
            var gunCanHitCurrentZone = damage.AffectedZones.Contains(CurrentZone);
            return IsDamageable && isInRange && gunCanHitCurrentZone;
        }

        public virtual bool IsPriorityTargetFor(PlayerDamage damage)
        {
            return false;
        }
    }
}
