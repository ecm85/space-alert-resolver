using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Players;
using BLL.Threats.Internal;

namespace BLL.ShipComponents
{
    public abstract class Zone
    {
        public int TotalDamage => CurrentDamageTokens.Count() + ExtraDamageTaken;
        private int ExtraDamageTaken { get; set; }

        public Gravolift Gravolift { get; }
        public IEnumerable<Player> Players => UpperStation.Players.Concat(LowerStation.Players).ToList();
        private IDictionary<InternalThreat, ZoneDebuff> DebuffsBySource { get; }
        private IList<DamageToken> CurrentDamageTokenList { get; }
        private IList<DamageToken> UnusedDamageTokenList { get; set; }
        public IEnumerable<DamageToken> CurrentDamageTokens => CurrentDamageTokenList;
        public IEnumerable<DamageToken> UnusedDamageTokens => UnusedDamageTokenList; 
        private bool HasStructuralCarryoverDamage { get; set; }

        public abstract ZoneLocation ZoneLocation { get; }
        public abstract UpperStation UpperStation { get; }
        public abstract LowerStation LowerStation { get; }

        protected Zone()
        {
            Gravolift = new Gravolift();
            DebuffsBySource = new Dictionary<InternalThreat, ZoneDebuff>();
            CurrentDamageTokenList = new List<DamageToken>();
            UnusedDamageTokenList = EnumFactory.All<DamageToken>().Shuffle(new Random()).ToList();
        }

        public ThreatDamageResult TakeAttack(int amount, ThreatDamageType damageType)
        {
            var damageShielded = 0;
            var damageDone = amount;
            if (damageType != ThreatDamageType.IgnoresShields)
            {
                damageShielded = UpperStation.ShieldThroughAttack(amount);
                damageDone -= damageShielded;
            }
            if (damageType == ThreatDamageType.DoubleDamageThroughShields)
                damageDone *= 2;
            if (damageShielded == 0 && damageDone > 0 && damageType == ThreatDamageType.Plasmatic)
                foreach (var player in Players)
                    player.KnockOut();
            var damageResult = TakeDamage(damageDone);
            damageResult.DamageShielded = damageShielded;
            return damageResult;
        }

        private ThreatDamageResult TakeDamage(int damage)
        {
            var damageDone = DebuffsBySource.Values
                .Where(debuff => debuff == ZoneDebuff.DoubleDamage)
                .Aggregate(damage, (current, doubleDamageDebuff) => current * 2);
            if (HasStructuralCarryoverDamage)
                damageDone *= 2;
            var newDamageTokens = UnusedDamageTokens.Take(Math.Min(damageDone, 6 - TotalDamage )).ToList();
            foreach (var newDamageToken in newDamageTokens)
                TakeDamage(newDamageToken);
            var extraDamageTaken = damageDone > newDamageTokens.Count ? damageDone - newDamageTokens.Count : 0;
            ExtraDamageTaken = extraDamageTaken;
            var shipDestroyed = extraDamageTaken > 0;
            return new ThreatDamageResult {ShipDestroyed = shipDestroyed, DamageShielded = 0};
        }

        public void TakeDamage(DamageToken newDamageToken, bool isCampaignDamage = false)
        {
            CurrentDamageTokenList.Add(newDamageToken);
            UnusedDamageTokenList.Remove(newDamageToken);
            var damageableComponent = GetDamageableComponent(newDamageToken);
            damageableComponent?.SetDamaged(isCampaignDamage);
            if (newDamageToken == DamageToken.Structural && isCampaignDamage)
                HasStructuralCarryoverDamage = true;
        }

        private IDamageableComponent GetDamageableComponent(DamageToken token)
        {
            switch (token)
            {
                case DamageToken.BackCannon:
                    return LowerStation.DamageableAlphaComponent;
                case DamageToken.FrontCannon:
                    return UpperStation.DamageableAlphaComponent;
                case DamageToken.Gravolift:
                    return Gravolift;
                case DamageToken.Reactor:
                    return LowerStation.DamageableBravoComponent;
                case DamageToken.Shield:
                    return UpperStation.DamageableBravoComponent;
                case DamageToken.Structural:
                    return null;
                default:
                    throw new InvalidOperationException("Invalid damage token encountered.");
            }
        }

        public int DrainShield(int? amount)
        {
            return UpperStation.DrainShield(amount);
        }

        public int DrainReactor(int? amount)
        {
            return LowerStation.DrainReactor(amount);
        }

        public void AddDebuff(ZoneDebuff debuff, InternalThreat source)
        {
            DebuffsBySource[source] = debuff;
            UpdateOptics();
            UpdateShields();
        }

        public void RemoveDebuffForSource(InternalThreat source)
        {
            DebuffsBySource.Remove(source);
            UpdateOptics();
            UpdateShields();
        }

        private void UpdateOptics()
        {
            var opticsDisrupted = DebuffsBySource.Values.Contains(ZoneDebuff.DisruptedOptics);
            UpperStation.SetOpticsDisrupted(opticsDisrupted);
            LowerStation.SetOpticsDisrupted(opticsDisrupted);
        }

        private void UpdateShields()
        {
            UpperStation.SetIneffectiveShields(DebuffsBySource.Values.Contains(ZoneDebuff.IneffectiveShields));
            UpperStation.SetReversedShields(DebuffsBySource.Values.Contains(ZoneDebuff.ReversedShields));
        }

        public int EnergyInReactor => LowerStation.EnergyInReactor;

        public void RepairFirstDamage(Player player)
        {
            if (!CurrentDamageTokens.Any())
                return;
            var damageRepairOrder = player.CurrentStation.StationLocation.IsUpperDeck() ?
                DamageTokenRepairOrderInUpperDeck :
                DamageTokenRepairOrderInLowerDeck;
            var damageToRepair = damageRepairOrder.First(damage => CurrentDamageTokens.Contains(damage));
            CurrentDamageTokenList.Remove(damageToRepair);
            UnusedDamageTokenList.Add(damageToRepair);
            UnusedDamageTokenList = UnusedDamageTokens.Shuffle().ToList();
            var component = GetDamageableComponent(damageToRepair);
            component?.Repair();
            if (damageToRepair == DamageToken.Structural)
                HasStructuralCarryoverDamage = false;
        }

        private static DamageToken[] DamageTokenRepairOrderInUpperDeck => new[]
        {
            DamageToken.FrontCannon,
            DamageToken.BackCannon,
            DamageToken.Gravolift,
            DamageToken.Shield,
            DamageToken.Reactor,
            DamageToken.Structural
        };

        private static DamageToken[] DamageTokenRepairOrderInLowerDeck => new[]
        {
            DamageToken.BackCannon,
            DamageToken.FrontCannon,
            DamageToken.Gravolift,
            DamageToken.Reactor,
            DamageToken.Shield,
            DamageToken.Structural
        };
    }
}
