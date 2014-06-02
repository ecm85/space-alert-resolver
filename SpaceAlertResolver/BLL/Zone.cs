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

		public ExternalThreatDamageResult TakeAttack(int damage)
		{
			var oldShields = UpperStation.EnergyContainer.Energy;
			UpperStation.EnergyContainer.Energy -= damage;
			var newShields = UpperStation.EnergyContainer.Energy;
			var damageShielded = oldShields - newShields;
			var damageDone = TakeDamage(damage - damageShielded);
			return new ExternalThreatDamageResult(damageDone)
			{
				DamageShielded = damageShielded
			};
		}

		public ThreatDamageResult TakeDamage(int damage)
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
			return new ThreatDamageResult
			{
				DamageDone = damageDone,
				ShipDestroyed = shipDestroyed
			};
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

		public void DrainShields()
		{
			UpperStation.EnergyContainer.Energy = 0;
		}
	}
}
