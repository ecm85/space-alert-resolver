using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats.Internal;

namespace BLL
{
	public class Zone
	{
		//TODO: Make UpperStation and LowerStation classes and have them have a reactor and shield instead of two EnergyContainers?
		public StandardStation UpperStation { get; set; }
		public StandardStation LowerStation { get; set; }
		public Gravolift Gravolift { get; set; }
		public int TotalDamage { get; private set; }
		public ZoneLocation ZoneLocation { get; set; }
		public IEnumerable<Player> Players { get { return UpperStation.Players.Concat(LowerStation.Players).ToList(); } }
		public IDictionary<InternalThreat, ZoneDebuff> DebuffsBySource { get; private set; }
		public List<DamageToken> CurrentDamageTokens  { get; private set; }
		public List<DamageToken> AllDamageTokensTaken { get; private set; }
		private readonly Random random = new Random();

		public Zone()
		{
			DebuffsBySource = new Dictionary<InternalThreat, ZoneDebuff>();
			CurrentDamageTokens = new List<DamageToken>();
			AllDamageTokensTaken = new List<DamageToken>();
		}

		public bool TakeAttack(int amount, ThreatDamageType damageType)
		{
			var oldShields = UpperStation.EnergyContainer.Energy;
			UpperStation.EnergyContainer.Energy -= amount;
			var newShields = UpperStation.EnergyContainer.Energy;
			var damageShielded = oldShields - newShields;
			var damageDone = amount - damageShielded;
			if (damageType == ThreatDamageType.DoubleDamageThroughShields)
				damageDone *= 2;
			if (damageShielded == 0 && damageDone > 0 && damageType == ThreatDamageType.Plasmatic)
				foreach (var player in Players)
					player.IsKnockedOut = true;
			return TakeDamage(damageDone);
		}

		public bool TakeDamage(int damage)
		{
			var damageDone = DebuffsBySource.Values
				.Where(debuff => debuff == ZoneDebuff.DoubleDamage)
				.Aggregate(damage, (current, doubleDamageDebuff) => current * 2);
			var newDamageTokens = GetNewDamageTokens(damageDone);
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
							LowerStation.EnergyContainer.SetDamaged();
							break;
						case DamageToken.Shield:
							UpperStation.EnergyContainer.SetDamaged();
							break;
						case DamageToken.Structural:
							break;
					}
				}
			}
			return shipDestroyed;
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
			var oldEnergy = UpperStation.EnergyContainer.Energy;
			UpperStation.EnergyContainer.Energy = 0;
			return oldEnergy;
		}

		public int DrainShield(int amount)
		{
			var oldEnergy = UpperStation.EnergyContainer.Energy;
			UpperStation.EnergyContainer.Energy -= amount;
			return oldEnergy;
		}

		public int DrainReactor()
		{
			var oldEnergy = LowerStation.EnergyContainer.Energy;
			LowerStation.EnergyContainer.Energy = 0;
			return oldEnergy;
		}

		public int DrainReactor(int amount)
		{
			var oldEnergy = LowerStation.EnergyContainer.Energy;
			LowerStation.EnergyContainer.Energy -= amount;
			return oldEnergy;
		}
	}
}
