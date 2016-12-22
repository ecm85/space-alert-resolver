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
		public static ZoneLocation RedwardZoneLocationWithWrapping(this ZoneLocation zoneLocation)
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

		public static ZoneLocation BluewardZoneLocationWithWrapping(this ZoneLocation zoneLocation)
		{
			switch (zoneLocation)
			{
				case ZoneLocation.Red:
					return ZoneLocation.White;
				case ZoneLocation.White:
					return ZoneLocation.Blue;
				case ZoneLocation.Blue:
					return ZoneLocation.Red;
				default:
					throw new InvalidOperationException("Invalid Zone Location encountered.");
			}
		}

		public static ZoneLocation? RedwardZoneLocation(this ZoneLocation? zoneLocation)
		{
			if (zoneLocation == null)
				return null;
			switch (zoneLocation)
			{
				case ZoneLocation.Blue:
					return ZoneLocation.White;
				case ZoneLocation.White:
					return ZoneLocation.Red;
				case ZoneLocation.Red:
					return null;
				default:
					throw new InvalidOperationException("Invalid Zone Location encountered.");
			}
		}

		public static ZoneLocation? BluewardZoneLocation(this ZoneLocation? zoneLocation)
		{
			if (zoneLocation == null)
				return null;
			switch (zoneLocation)
			{
				case ZoneLocation.Red:
					return ZoneLocation.White;
				case ZoneLocation.White:
					return ZoneLocation.Blue;
				case ZoneLocation.Blue:
					return null;
				default:
					throw new InvalidOperationException("Invalid Zone Location encountered.");
			}
		}

		public static StationLocation GetUpperStation(this ZoneLocation zoneLocation)
		{
			switch (zoneLocation)
			{
				case ZoneLocation.Red:
					return StationLocation.UpperRed;
				case ZoneLocation.White:
					return StationLocation.UpperWhite;
				case ZoneLocation.Blue:
					return StationLocation.UpperBlue;
				default:
					throw new InvalidOperationException("Invalid Zone Location encountered.");
			}
		}

		public static StationLocation GetLowerStation(this ZoneLocation zoneLocation)
		{
			switch (zoneLocation)
			{
				case ZoneLocation.Red:
					return StationLocation.LowerRed;
				case ZoneLocation.White:
					return StationLocation.LowerWhite;
				case ZoneLocation.Blue:
					return StationLocation.LowerBlue;
				default:
					throw new InvalidOperationException("Invalid Zone Location encountered.");
			}
		}
	}
}
