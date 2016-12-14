using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BLL.Tracks
{
	[SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
	public enum TrackConfiguration
	{
		Track1 = 1,
		Track2,
		Track3,
		Track4,
		Track5,
		Track6,
		Track7
	}

	public static class TrackConfigurationExtensions
	{
		public static string DisplayName(this TrackConfiguration trackConfiguration)
		{
			switch (trackConfiguration)
			{
				case TrackConfiguration.Track1:
					return "Track 1 (T1)";
				case TrackConfiguration.Track2:
					return "Track 2 (T2)";
				case TrackConfiguration.Track3:
					return "Track 3 (T3)";
				case TrackConfiguration.Track4:
					return "Track 4 (T4)";
				case TrackConfiguration.Track5:
					return "Track 5 (T5)";
				case TrackConfiguration.Track6:
					return "Track 6 (T6)";
				case TrackConfiguration.Track7:
					return "Track 7 (T7)";
				default:
					throw new InvalidOperationException();
			}
		}

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

		public static IDictionary<int, TrackBreakpointType> TrackBreakpoints(this TrackConfiguration trackConfiguration)
		{
			switch (trackConfiguration)
			{
				case TrackConfiguration.Track1:
					return new Dictionary<int, TrackBreakpointType>
					{
						{5, TrackBreakpointType.X},
						{1, TrackBreakpointType.Z}
					};
				case TrackConfiguration.Track2:
					return new Dictionary<int, TrackBreakpointType>
					{
						{8, TrackBreakpointType.X},
						{1, TrackBreakpointType.Z}
					};
				case TrackConfiguration.Track3:
					return new Dictionary<int, TrackBreakpointType>
					{
						{8, TrackBreakpointType.X},
						{3, TrackBreakpointType.Y},
						{1, TrackBreakpointType.Z}
					};
				case TrackConfiguration.Track4:
					return new Dictionary<int, TrackBreakpointType>
					{
						{9, TrackBreakpointType.X},
						{5, TrackBreakpointType.Y},
						{1, TrackBreakpointType.Z}
					};
				case TrackConfiguration.Track5:
					return new Dictionary<int, TrackBreakpointType>
					{
						{11, TrackBreakpointType.X},
						{7, TrackBreakpointType.Y},
						{1, TrackBreakpointType.Z}
					};
				case TrackConfiguration.Track6:
					return new Dictionary<int, TrackBreakpointType>
					{
						{10, TrackBreakpointType.X},
						{7, TrackBreakpointType.Y},
						{3, TrackBreakpointType.Y},
						{1, TrackBreakpointType.Z}
					};
				case TrackConfiguration.Track7:
					return new Dictionary<int, TrackBreakpointType>
					{
						{12, TrackBreakpointType.X},
						{8, TrackBreakpointType.Y},
						{5, TrackBreakpointType.Y},
						{1, TrackBreakpointType.Z}
					};
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
