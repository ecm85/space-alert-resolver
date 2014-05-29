using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Tracks
{
	public class Track6 : Track
	{
		public Track6(Zone zone)
			: base(GetBreakpoints(), zone, GetSections())
		{
		}

		private static IList<TrackSection> GetSections()
		{
			return new []
			{
				new TrackSection {DistanceFromShip = 3, Length = 5},
				new TrackSection {DistanceFromShip = 2, Length = 5},
				new TrackSection {DistanceFromShip = 1, Length = 5}
			};
		}

		private static IEnumerable<TrackBreakpoint> GetBreakpoints()
		{
			return new[]
			{
				new TrackBreakpoint{Position = 5, Type = TrackBreakpointType.X},
				new TrackBreakpoint{Position = 8, Type = TrackBreakpointType.Y},
				new TrackBreakpoint{Position = 12, Type = TrackBreakpointType.Y},
				new TrackBreakpoint{Position = 14, Type = TrackBreakpointType.Z}
			};
		}
	}
}
