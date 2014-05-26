using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public class DamageResult
	{
		public int DamageDone { get; set; }
		public int DamageShielded { get; set; }

		public void AddDamage(DamageResult damageResult)
		{
			DamageDone += damageResult.DamageDone;
			DamageShielded += damageResult.DamageShielded;
		}
	}
}
