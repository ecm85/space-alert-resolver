using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Tracks
{
	public enum TrackConfiguration
	{
		Track1,
		Track2,
		Track3,
		Track4,
		Track5,
		Track6,
		Track7
	}

	public static class TrackConfigurationExtensions
	{
		public static IList<TrackSection> TrackSections(this TrackConfiguration trackConfiguration)
		{
			switch (trackConfiguration)
			{
				case TrackConfiguration.Track1:
					return new[]
					{
						new TrackSection {DistanceFromShip = 2, Length = 5},
						new TrackSection {DistanceFromShip = 1, Length = 5}
					};
				case TrackConfiguration.Track2:
					return new[]
					{
						new TrackSection {DistanceFromShip = 3, Length = 1},
						new TrackSection {DistanceFromShip = 2, Length = 5},
						new TrackSection {DistanceFromShip = 1, Length = 5}
					};
				case TrackConfiguration.Track3:
					return new[]
					{
						new TrackSection {DistanceFromShip = 3, Length = 2},
						new TrackSection {DistanceFromShip = 2, Length = 5},
						new TrackSection {DistanceFromShip = 1, Length = 5}
					};
				case TrackConfiguration.Track4:
					return new[]
					{
						new TrackSection {DistanceFromShip = 3, Length = 3},
						new TrackSection {DistanceFromShip = 2, Length = 5},
						new TrackSection {DistanceFromShip = 1, Length = 5}
					};
				case TrackConfiguration.Track5:
					return new[]
					{
						new TrackSection {DistanceFromShip = 3, Length = 4},
						new TrackSection {DistanceFromShip = 2, Length = 5},
						new TrackSection {DistanceFromShip = 1, Length = 5}
					};
				case TrackConfiguration.Track6:
					return new[]
					{
						new TrackSection {DistanceFromShip = 3, Length = 5},
						new TrackSection {DistanceFromShip = 2, Length = 5},
						new TrackSection {DistanceFromShip = 1, Length = 5}
					};
				case TrackConfiguration.Track7:
					return new[]
					{
						new TrackSection {DistanceFromShip = 3, Length = 6},
						new TrackSection {DistanceFromShip = 2, Length = 5},
						new TrackSection {DistanceFromShip = 1, Length = 5}
					};
				default:
					throw new InvalidOperationException();
			}
		}

		public static IList<TrackBreakpoint> TrackBreakpoints(this TrackConfiguration trackConfiguration)
		{
			switch (trackConfiguration)
			{
				case TrackConfiguration.Track1:
					return new[]
					{
						new TrackBreakpoint{Position = 8, Type = TrackBreakpointType.X},
						new TrackBreakpoint{Position = 1, Type = TrackBreakpointType.Z}
					};
				case TrackConfiguration.Track2:
					return new[]
					{
						new TrackBreakpoint{Position = 5, Type = TrackBreakpointType.X},
						new TrackBreakpoint{Position = 1, Type = TrackBreakpointType.Z}
					};
				case TrackConfiguration.Track3:
					return new[]
					{
						new TrackBreakpoint{Position = 8, Type = TrackBreakpointType.X},
						new TrackBreakpoint{Position = 3, Type = TrackBreakpointType.Y},
						new TrackBreakpoint{Position = 1, Type = TrackBreakpointType.Z}
					};
				case TrackConfiguration.Track4:
					return new[]
					{
						new TrackBreakpoint{Position = 9, Type = TrackBreakpointType.X},
						new TrackBreakpoint{Position = 5, Type = TrackBreakpointType.Y},
						new TrackBreakpoint{Position = 1, Type = TrackBreakpointType.Z}
					};
				case TrackConfiguration.Track5:
					return new[]
					{
						new TrackBreakpoint{Position = 11, Type = TrackBreakpointType.X},
						new TrackBreakpoint{Position = 7, Type = TrackBreakpointType.Y},
						new TrackBreakpoint{Position = 1, Type = TrackBreakpointType.Z}
					};
				case TrackConfiguration.Track6:
					return new[]
					{
						new TrackBreakpoint{Position = 10, Type = TrackBreakpointType.X},
						new TrackBreakpoint{Position = 7, Type = TrackBreakpointType.Y},
						new TrackBreakpoint{Position = 3, Type = TrackBreakpointType.Y},
						new TrackBreakpoint{Position = 1, Type = TrackBreakpointType.Z}
					};
				case TrackConfiguration.Track7:
					return new[]
					{
						new TrackBreakpoint{Position = 12, Type = TrackBreakpointType.X},
						new TrackBreakpoint{Position = 8, Type = TrackBreakpointType.Y},
						new TrackBreakpoint{Position = 5, Type = TrackBreakpointType.Y},
						new TrackBreakpoint{Position = 1, Type = TrackBreakpointType.Z}
					};
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
