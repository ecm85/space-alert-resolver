using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public class Damage
	{
		public int Amount { get; set; }
		public DamageType DamageType { get; set; }
		public ZoneType[] ZoneTypesAffected { get; set; }
		public int Range { get; set; }
	}
}
