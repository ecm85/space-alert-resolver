using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Tracks
{
	public class Track5 : Track
	{
		public Track5(Zone zone)
			: base(GetBreakpoints(), zone, GetSections())
		{
		}

		private static IList<TrackSection> GetSections()
		{
			return new []
			{
				new TrackSection {DistanceFromShip = 3, Length = 4},
				new TrackSection {DistanceFromShip = 2, Length = 5},
				new TrackSection {DistanceFromShip = 1, Length = 5}
			};
		}

		private static IEnumerable<TrackBreakpoint> GetBreakpoints()
		{
			return new[]
			{
				new TrackBreakpoint{Position = 3, Type = TrackBreakpointType.X},
				new TrackBreakpoint{Position = 7, Type = TrackBreakpointType.Y},
				new TrackBreakpoint{Position = 13, Type = TrackBreakpointType.Z}
			};
		}
	}
}
