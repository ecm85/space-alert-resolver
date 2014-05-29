using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Tracks
{
	public class Track1 : Track
	{
		public Track1(Zone zone)
			: base(GetBreakpoints(), zone, GetSections())
		{
		}

		private static IList<TrackSection> GetSections()
		{
			return new []
			{
				new TrackSection {DistanceFromShip = 2, Length = 5},
				new TrackSection {DistanceFromShip = 1, Length = 5}
			};
		}

		private static IEnumerable<TrackBreakpoint> GetBreakpoints()
		{
			return new []
			{
				new TrackBreakpoint{Position = 5, Type = TrackBreakpointType.X},
				new TrackBreakpoint{Position = 9, Type = TrackBreakpointType.Z}
			};
		}
	}
}
