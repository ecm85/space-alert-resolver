using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Tracks
{
	public class Track
	{
		private readonly IDictionary<int, TrackBreakpointType> breakpoints;
		private readonly IList<TrackSection> sections;

		public Track(TrackConfiguration trackConfiguration)
		{
			breakpoints = trackConfiguration.TrackBreakpoints();
			sections = trackConfiguration.TrackSections();
		}

		public int GetStartingPosition()
		{
			return sections.Sum(section => section.Length);
		}

		public TrackBreakpointType? MoveSingle(int currentPosition)
		{
			var newPosition = currentPosition;
			newPosition--;
			return breakpoints.ContainsKey(newPosition) ? breakpoints[newPosition] : (TrackBreakpointType?)null;
		}

		public int DistanceToThreat(int position)
		{
			var distance = position;
			foreach (var section in sections.OrderBy(section => section.DistanceFromShip))
			{
				if (section.Length >= distance)
					return section.DistanceFromShip;
				distance -= section.Length;
			}
			throw new InvalidOperationException();
		}
	}
}
