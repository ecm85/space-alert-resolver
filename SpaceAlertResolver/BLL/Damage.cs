using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public abstract class Damage
	{
		public int Amount { get; private set; }
		public DamageType DamageType { get; private set; }

		protected Damage(int amount, DamageType damageType)
		{
			Amount = amount;
			DamageType = damageType;
		}
	}
}
