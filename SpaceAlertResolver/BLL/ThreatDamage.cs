using BLL.ShipComponents;

namespace BLL
{
	public class ThreatDamage
	{
		public int Amount { get; private set; }
		public ThreatDamageType ThreatDamageType { get; private set; }
		public ZoneLocation ZoneLocation { get; private set; }
		public int? DistanceToSource { get; private set; }
		public int DamageShielded { get; set; }

		internal ThreatDamage(int amount, ThreatDamageType threatDamageType, ZoneLocation zoneLocation, int? distanceToSource = null)
		{
			Amount = amount;
			ThreatDamageType = threatDamageType;
			ZoneLocation = zoneLocation;
			DistanceToSource = distanceToSource;
		}
	}
}
