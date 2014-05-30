using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public class ThreatDamage : Damage
	{
		public IEnumerable<Zone> ZonesAffected { get; private set; }

		public ThreatDamage(int amount, IEnumerable<Zone> zonesAffected)
			: base(amount, DamageType.ExternalThreat)
		{
			Amount = amount;
			ZonesAffected = zonesAffected;
		}
	}
}
