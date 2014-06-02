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

		public Zone()
		{
			DebuffsBySource = new Dictionary<InternalThreat, ZoneDebuff>();
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
			//TODO: Apply damageDone tokens
			TotalDamage += damageDone;
			return new ThreatDamageResult
			{
				DamageDone = damageDone,
				ShipDestroyed = TotalDamage >= 7
			};
		}

		public void DrainShields()
		{
			UpperStation.EnergyContainer.Energy = 0;
		}
	}
}
