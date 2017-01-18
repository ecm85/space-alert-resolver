using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Threats.Internal;

namespace BLL.ShipComponents
{
	public abstract class Zone
	{
		public int TotalDamage { get; private set; }
		public Gravolift Gravolift { get; }
		public IEnumerable<Player> Players => UpperStation.Players.Concat(LowerStation.Players).ToList();
		private IDictionary<InternalThreat, ZoneDebuff> DebuffsBySource { get; }
		public List<DamageToken> CurrentDamageTokens  { get; }
		public List<DamageToken> AllDamageTokensTaken { get; }
		private static readonly Random random = new Random();

		public abstract ZoneLocation ZoneLocation { get; }
		public abstract UpperStation UpperStation { get; }
		public abstract LowerStation LowerStation { get; }

		protected Zone()
		{
			Gravolift = new Gravolift();
			DebuffsBySource = new Dictionary<InternalThreat, ZoneDebuff>();
			CurrentDamageTokens = new List<DamageToken>();
			AllDamageTokensTaken = new List<DamageToken>();
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
					player.IsKnockedOut = true;
			var damageResult = TakeDamage(damageDone);
			damageResult.DamageShielded = damageShielded;
			return damageResult;
		}

		private ThreatDamageResult TakeDamage(int damage)
		{
			var damageDone = DebuffsBySource.Values
				.Where(debuff => debuff == ZoneDebuff.DoubleDamage)
				.Aggregate(damage, (current, doubleDamageDebuff) => current * 2);
			var newDamageTokens = GetNewDamageTokens(Math.Min(damageDone, 6 - TotalDamage ));
			CurrentDamageTokens.AddRange(newDamageTokens);
			AllDamageTokensTaken.AddRange(newDamageTokens);
			TotalDamage += damageDone;
			var shipDestroyed = TotalDamage >= 7;
			foreach (var token in newDamageTokens)
			{
				var damageableComponent = GetDamageableComponent(token);
				damageableComponent?.SetDamaged();
			}
			return new ThreatDamageResult {ShipDestroyed = shipDestroyed, DamageShielded = 0};
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

		private IList<DamageToken> GetNewDamageTokens(int count)
		{
			var openTokens = EnumFactory.All<DamageToken>().Except(CurrentDamageTokens).ToList();
			var newDamageTokens = new List<DamageToken>();
			for (var i = 0; i < count; i++)
			{
				var selectedToken = openTokens[random.Next(openTokens.Count)];
				openTokens.Remove(selectedToken);
				newDamageTokens.Add(selectedToken);
			}
			return newDamageTokens;
		}

		public int DrainShield()
		{
			return UpperStation.DrainShield();
		}

		public void DrainShield(int amount)
		{
			UpperStation.DrainShield(amount);
		}

		public int DrainReactor()
		{
			return LowerStation.EmptyReactor();
		}

		public int DrainReactor(int amount)
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
			CurrentDamageTokens.Remove(damageToRepair);
			var component = GetDamageableComponent(damageToRepair);
			component.Repair();
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
