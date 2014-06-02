using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public class ExternalThreatDamageResult : ThreatDamageResult
	{
		public int DamageShielded { get; set; }

		public ExternalThreatDamageResult()
		{
		}

		public ExternalThreatDamageResult(ThreatDamageResult other)
		{
			DamageDone = other.DamageDone;
			ShipDestroyed = other.ShipDestroyed;
		}

		public void AddDamage(ExternalThreatDamageResult other)
		{
			base.AddDamage(other);
;			DamageShielded += other.DamageShielded;
		}
	}
}
