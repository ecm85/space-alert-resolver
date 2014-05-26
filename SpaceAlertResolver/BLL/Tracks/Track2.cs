using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Tracks
{
	public class Track2 : Track
	{
		public Track2(ZoneType zoneType)
			: base(GetBreakpoints(), zoneType, GetSections())
		{
		}

		private static IList<TrackSection> GetSections()
		{
			return new []
			{
				new TrackSection {DistanceFromShip = 3, Length = 1},
				new TrackSection {DistanceFromShip = 2, Length = 5},
				new TrackSection {DistanceFromShip = 1, Length = 5}
			};
		}

		private static IEnumerable<TrackBreakpoint> GetBreakpoints()
		{
			return new[]
			{
				new TrackBreakpoint{Position = 3, Type = TrackBreakpointType.X},
				new TrackBreakpoint{Position = 10, Type = TrackBreakpointType.Z}
			};
		}
	}
}
