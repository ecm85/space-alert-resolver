using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Tracks
{
	public class Track3 : Track
	{
		public Track3(Zone zone)
			: base(GetBreakpoints(), zone, GetSections())
		{
		}

		private static IList<TrackSection> GetSections()
		{
			return new []
			{
				new TrackSection {DistanceFromShip = 3, Length = 2},
				new TrackSection {DistanceFromShip = 2, Length = 5},
				new TrackSection {DistanceFromShip = 1, Length = 5}
			};
		}

		private static IEnumerable<TrackBreakpoint> GetBreakpoints()
		{
			return new[]
			{
				new TrackBreakpoint{Position = 4, Type = TrackBreakpointType.X},
				new TrackBreakpoint{Position = 9, Type = TrackBreakpointType.Y},
				new TrackBreakpoint{Position = 11, Type = TrackBreakpointType.Z}
			};
		}
	}
}
