using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public class ExternalThreatDamageResult
	{
		public int DamageDone { get; set; }
		public int DamageShielded { get; set; }

		public void AddDamage(ExternalThreatDamageResult other)
		{
			DamageDone += other.DamageDone;
			DamageShielded += other.DamageShielded;
		}
	}
}
