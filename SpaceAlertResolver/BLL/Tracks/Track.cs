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

		public IEnumerable<TrackBreakpointType> GetCrossedBreakpoints(int oldPosition, int newPosition)
		{
			var crossedBreakpoints = new List<TrackBreakpointType>();
			for(var i = oldPosition - 1; i >= newPosition; i--)
				if(breakpoints.ContainsKey(i))
					crossedBreakpoints.Add(breakpoints[i]);
			return crossedBreakpoints;
		}
	}
}
