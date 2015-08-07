using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Threats.Internal;

namespace BLL.ShipComponents
{
	public class Zone
	{
		public UpperStation UpperStation { get; set; }
		public LowerStation LowerStation { get; set; }
		public int TotalDamage { get; private set; }
		public ZoneLocation ZoneLocation { get; set; }
		public Gravolift Gravolift { get; set; }
		public IEnumerable<Player> Players { get { return UpperStation.Players.Concat(LowerStation.Players).ToList(); } }
		private IDictionary<InternalThreat, ZoneDebuff> DebuffsBySource { get; set; }
		public List<DamageToken> CurrentDamageTokens  { get; private set; }
		public List<DamageToken> AllDamageTokensTaken { get; private set; }
		private readonly Random random = new Random();

		public Zone()
		{
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
			if (!shipDestroyed)
			{
				foreach (var token in newDamageTokens)
				{
					var damageableComponent = GetDamageableComponent(token);
					if (damageableComponent != null)
						damageableComponent.SetDamaged();
				}
			}
			return new ThreatDamageResult {ShipDestroyed = shipDestroyed, DamageShielded = 0};
		}

		private IDamageableComponent GetDamageableComponent(DamageToken token)
		{
			switch (token)
			{
				case DamageToken.BackCannon:
					return LowerStation.GetDamageableAlphaComponent();
				case DamageToken.FrontCannon:
					return UpperStation.GetDamageableAlphaComponent();
				case DamageToken.Gravolift:
					return Gravolift;
				case DamageToken.Reactor:
					return LowerStation.GetDamageableBravoComponent();
				case DamageToken.Shield:
					return UpperStation.GetDamageableBravoComponent();
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
			return LowerStation.DrainReactor();
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

		public int GetEnergyInReactor()
		{
			return LowerStation.GetEnergyInReactor();
		}

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

		private static DamageToken[] DamageTokenRepairOrderInUpperDeck
		{
			get
			{
				return new[]
				{
					DamageToken.FrontCannon,
					DamageToken.BackCannon,
					DamageToken.Gravolift,
					DamageToken.Shield,
					DamageToken.Reactor,
					DamageToken.Structural
				};
			}
		}

		private static DamageToken[] DamageTokenRepairOrderInLowerDeck
		{
			get
			{
				return new[]
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
	}
}
