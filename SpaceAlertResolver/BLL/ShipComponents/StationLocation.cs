using System;

namespace BLL.ShipComponents
{
	public enum StationLocation
	{
		UpperRed,
		LowerRed,
		UpperWhite,
		LowerWhite,
		UpperBlue,
		LowerBlue,
		Interceptor
	}

	public static class StationLocationExtensions
	{
		public static ZoneLocation ZoneLocation(this StationLocation stationLocation)
		{
			switch (stationLocation)
			{
				case StationLocation.UpperBlue:
				case StationLocation.LowerBlue:
					return ShipComponents.ZoneLocation.Blue;
				case StationLocation.UpperWhite:
				case StationLocation.LowerWhite:
					return ShipComponents.ZoneLocation.White;
				case StationLocation.UpperRed:
				case StationLocation.LowerRed:
					return ShipComponents.ZoneLocation.Red;
				//case StationLocation.Interceptor: //TODO: ??
				default:
					throw new InvalidOperationException();
			}
		}

		public static StationLocation? RedwardStationLocation(this StationLocation stationLocation)
		{
			switch (stationLocation)
			{
				case StationLocation.UpperBlue:
					return StationLocation.UpperWhite;
				case StationLocation.UpperWhite:
					return StationLocation.UpperRed;
				case StationLocation.LowerBlue:
					return StationLocation.LowerWhite;
				case StationLocation.LowerWhite:
					return StationLocation.LowerRed;
				default:
					return null;
			}
		}

		public static StationLocation? BluewardStationLocation(this StationLocation stationLocation)
		{
			switch (stationLocation)
			{
				case StationLocation.UpperRed:
					return StationLocation.UpperWhite;
				case StationLocation.UpperWhite:
					return StationLocation.UpperBlue;
				case StationLocation.LowerRed:
					return StationLocation.LowerWhite;
				case StationLocation.LowerWhite:
					return StationLocation.LowerBlue;
				default:
					return null;
			}
		}

		public static StationLocation? OppositeStationLocation(this StationLocation stationLocation)
		{
			switch (stationLocation)
			{
				case StationLocation.UpperRed:
					return StationLocation.LowerRed;
				case StationLocation.UpperWhite:
					return StationLocation.LowerWhite;
				case StationLocation.UpperBlue:
					return StationLocation.LowerBlue;
				case StationLocation.LowerRed:
					return StationLocation.UpperRed;
				case StationLocation.LowerWhite:
					return StationLocation.UpperWhite;
				case StationLocation.LowerBlue:
					return StationLocation.UpperBlue;
				default:
					return null;
			}
		}
	}
}
