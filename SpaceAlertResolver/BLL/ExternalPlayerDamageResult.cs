using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public class ExternalPlayerDamageResult
	{
		public int DamageDone { get; set; }
		public int DamageShielded { get; set; }

		public void AddDamage(ExternalPlayerDamageResult other)
		{
			DamageDone += other.DamageDone;
			DamageShielded += other.DamageShielded;
		}
	}
}
