using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
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
					return BLL.ZoneLocation.Blue;
				case StationLocation.UpperWhite:
				case StationLocation.LowerWhite:
					return BLL.ZoneLocation.White;
				case StationLocation.UpperRed:
				case StationLocation.LowerRed:
					return BLL.ZoneLocation.Red;
				//case StationLocation.Interceptor: //TODO: ??
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
