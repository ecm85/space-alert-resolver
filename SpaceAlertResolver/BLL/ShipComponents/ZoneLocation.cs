using System;

namespace BLL.ShipComponents
{
	public enum ZoneLocation
	{
		Red,
		White,
		Blue
	}

	public static class ZoneLocationExtensions
	{
		public static ZoneLocation RedwardZoneLocation(this ZoneLocation zoneLocation)
		{
			switch (zoneLocation)
			{
				case ZoneLocation.Blue:
					return ZoneLocation.White;
				case ZoneLocation.White:
					return ZoneLocation.Red;
				case ZoneLocation.Red:
					return ZoneLocation.Blue;
				default:
					throw new InvalidOperationException("Invalid Zone Location encountered.");
			}
		}

		public static ZoneLocation BluewardZoneLocation(this ZoneLocation zoneLocation)
		{
			switch (zoneLocation)
			{
				case ZoneLocation.Blue:
					return ZoneLocation.Red;
				case ZoneLocation.White:
					return ZoneLocation.Blue;
				case ZoneLocation.Red:
					return ZoneLocation.White;
				default:
					throw new InvalidOperationException("Invalid Zone Location encountered.");
			}
		}
	}
}
