using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public class PlayerDamage : Damage
	{
		public int Range { get; private set; }
		public IList<ZoneLocation> ZoneLocations { get; private set; }

		public PlayerDamage(int amount, PlayerDamageType playerDamageType, int range, IList<ZoneLocation> zoneLocations)
			: base(amount, playerDamageType)
		{
			Range = range;
			ZoneLocations = zoneLocations;
		}
	}
}
