using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public class ThreatDamageResult
	{
		public int DamageDone { get; set; }
		public bool ShipDestroyed { get; set; }

		public void AddDamage(ThreatDamageResult other)
		{
			DamageDone += other.DamageDone;
			ShipDestroyed = ShipDestroyed || other.ShipDestroyed;
		}
	}
}
