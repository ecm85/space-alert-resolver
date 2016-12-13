using System.Collections.Generic;
using BLL.ShipComponents;

namespace BLL
{
	public class ThreatDamage
	{
		public int Amount { get; private set; }
		public ThreatDamageType ThreatDamageType { get; private set; }
		public IList<ZoneLocation> ZoneLocations { get; private set; }
		public int? DistanceToSource { get; private set; }

		public ThreatDamage(int amount, ThreatDamageType threatDamageType, IList<ZoneLocation> zoneLocations, int? distanceToSource = null)
		{
			Amount = amount;
			ThreatDamageType = threatDamageType;
			ZoneLocations = zoneLocations;
			DistanceToSource = distanceToSource;
		}
	}
}
