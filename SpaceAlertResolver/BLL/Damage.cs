using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public abstract class Damage
	{
		public int Amount { get; private set; }
		public PlayerDamageType PlayerDamageType { get; private set; }

		protected Damage(int amount, PlayerDamageType playerDamageType)
		{
			Amount = amount;
			PlayerDamageType = playerDamageType;
		}
	}
}
