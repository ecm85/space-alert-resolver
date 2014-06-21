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
				var reversedShields = DebuffsBySource.Values.Contains(ZoneDebuff.ReversedShields);
				var ineffectiveShields = DebuffsBySource.Values.Contains(ZoneDebuff.IneffectiveShields);
				if (reversedShields)
				{
					damageDone += UpperStation.Shield.Energy;
					UpperStation.Shield.Energy = 0;
				}
				else if(!ineffectiveShields)
				{
					var oldShields = UpperStation.Shield.Energy;
					UpperStation.Shield.Energy -= amount;
					var newShields = UpperStation.Shield.Energy;
					damageShielded = oldShields - newShields;
					damageDone -= damageShielded;
				}
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
					switch (token)
					{
						case DamageToken.BackCannon:
							LowerStation.Cannon.SetDamaged();
							break;
						case DamageToken.FrontCannon:
							UpperStation.Cannon.SetDamaged();
							break;
						case DamageToken.Gravolift:
							Gravolift.SetDamaged();
							break;
						case DamageToken.Reactor:
							LowerStation.Reactor.SetDamaged();
							break;
						case DamageToken.Shield:
							UpperStation.Shield.SetDamaged();
							break;
						case DamageToken.Structural:
							break;
					}
				}
			}
			return new ThreatDamageResult {ShipDestroyed = shipDestroyed, DamageShielded = 0};
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
			var oldEnergy = UpperStation.Shield.Energy;
			UpperStation.Shield.Energy = 0;
			return oldEnergy;
		}

		public void DrainShield(int amount)
		{
			UpperStation.Shield.Energy -= amount;
		}

		public int DrainReactor()
		{
			var oldEnergy = LowerStation.Reactor.Energy;
			LowerStation.Reactor.Energy = 0;
			var currentEnergy = LowerStation.Reactor.Energy;
			return oldEnergy - currentEnergy;
		}

		public int DrainReactor(int amount)
		{
			var oldEnergy = LowerStation.Reactor.Energy;
			LowerStation.Reactor.Energy -= amount;
			var currentEnergy = LowerStation.Reactor.Energy;
			return oldEnergy - currentEnergy;
		}

		public void AddDebuff(ZoneDebuff debuff, InternalThreat source)
		{
			DebuffsBySource[source] = debuff;
			UpdateOptics();
		}

		private void UpdateOptics()
		{
			var opticsDisrupted = DebuffsBySource.Values.Contains(ZoneDebuff.DisruptedOptics);
			UpperStation.Cannon.DisruptedOptics = opticsDisrupted;
			LowerStation.Cannon.DisruptedOptics = opticsDisrupted;
		}

		public void RemoveDebuffForSource(InternalThreat source)
		{
			DebuffsBySource.Remove(source);
			UpdateOptics();
		}

		public int GetEnergyInReactor()
		{
			return LowerStation.Reactor.Energy;
		}
	}
}
