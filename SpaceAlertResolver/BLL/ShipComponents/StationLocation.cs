using System;
using System.Linq;

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
		Interceptor1,
		Interceptor2,
		Interceptor3
	}

	public static class StationLocationExtensions
	{
		public static bool IsOnShip(this StationLocation stationLocation)
		{
			switch (stationLocation)
			{
				case StationLocation.UpperBlue:
				case StationLocation.LowerBlue:
				case StationLocation.UpperWhite:
				case StationLocation.LowerWhite:
				case StationLocation.UpperRed:
				case StationLocation.LowerRed:
					return true;
				case StationLocation.Interceptor1:
				case StationLocation.Interceptor2:
				case StationLocation.Interceptor3:
					return false;
				default:
					throw new InvalidOperationException("Invalid StationLocation encountered.");
			}
		}

		public static bool IsInterceptorStationLocation(this StationLocation stationLocation)
		{
			return !stationLocation.IsOnShip();
		}

		public static bool IsUpperDeck(this StationLocation stationLocation)
		{
			var upperDeckStations = new[] {StationLocation.UpperBlue, StationLocation.UpperWhite, StationLocation.UpperRed};
			return upperDeckStations.Contains(stationLocation);
		}

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
				case StationLocation.Interceptor1:
				case StationLocation.Interceptor2:
				case StationLocation.Interceptor3:
					throw new InvalidOperationException("Cannot get zone location for interceptor station.");
				default:
					throw new InvalidOperationException("Invalid StationLocation encountered.");
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

		public static StationLocation? ShipwardLocation(this StationLocation stationLocation)
		{
			switch (stationLocation)
			{
				case StationLocation.Interceptor3:
					return StationLocation.Interceptor2;
				case StationLocation.Interceptor2:
					return StationLocation.Interceptor1;
				case StationLocation.Interceptor1:
					return StationLocation.UpperRed;
				default:
					return null;
			}
		}

		public static StationLocation? SpacewardLocation(this StationLocation stationLocation)
		{
			switch (stationLocation)
			{
				case StationLocation.UpperRed:
					return StationLocation.Interceptor1;
				case StationLocation.Interceptor1:
					return StationLocation.Interceptor2;
				case StationLocation.Interceptor2:
					return StationLocation.Interceptor3;
				default:
					return null;
			}
		}

		public static StationLocation? DiagonalStation(this StationLocation stationLocation)
		{
			switch (stationLocation)
			{
				case StationLocation.LowerBlue:
					return StationLocation.UpperRed;
				case StationLocation.UpperBlue:
					return StationLocation.LowerRed;
				case StationLocation.LowerRed:
					return StationLocation.UpperBlue;
				case StationLocation.UpperRed:
					return StationLocation.LowerBlue;
				default:
					return null;
			}
		}

		public static int? DistanceFromShip(this StationLocation stationLocation)
		{
			switch (stationLocation)
			{
				case StationLocation.Interceptor3:
					return 3;
				case StationLocation.Interceptor2:
					return 2;
				case StationLocation.Interceptor1:
					return 1;
				default:
					return null;
			}
		}
	}
}
