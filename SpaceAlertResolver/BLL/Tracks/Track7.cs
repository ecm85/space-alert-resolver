using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Tracks
{
	public class Track7 : Track
	{
		public Track7(ZoneType zoneType)
			: base(GetBreakpoints(), zoneType, GetSections())
		{
		}

		private static IList<TrackSection> GetSections()
		{
			return new[]
			{
				new TrackSection {DistanceFromShip = 3, Length = 6},
				new TrackSection {DistanceFromShip = 2, Length = 5},
				new TrackSection {DistanceFromShip = 1, Length = 5}
			};
		}

		private static IEnumerable<TrackBreakpoint> GetBreakpoints()
		{
			return new[]
			{
				new TrackBreakpoint{Position = 4, Type = TrackBreakpointType.X},
				new TrackBreakpoint{Position = 8, Type = TrackBreakpointType.Y},
				new TrackBreakpoint{Position = 11, Type = TrackBreakpointType.Y},
				new TrackBreakpoint{Position = 15, Type = TrackBreakpointType.Z}
			};
		}
	}
}
