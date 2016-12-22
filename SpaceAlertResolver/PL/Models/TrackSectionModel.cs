using System.Collections.Generic;
using System.Linq;
using BLL.Threats;
using BLL.Tracks;

namespace PL.Models
{
	public class TrackSectionModel
	{
		public int DistanceFromShip { get; set; }
		public IEnumerable<TrackSpaceModel> TrackSpaces { get; set; }
		public TrackSectionModel(TrackSection section, IEnumerable<Threat> threatsOnTrack, IDictionary<int, TrackBreakpointType> breakpoints)
		{
			TrackSpaces = Enumerable.Range(1 + 5 * (section.DistanceFromShip - 1), section.Length)
				.Reverse()
				.Select(space => new TrackSpaceModel
				{
					Space = space,
					HasAnyThreats = threatsOnTrack.Any(threat => threat.Position == space),
					Breakpoint = breakpoints.ContainsKey(space) ? breakpoints[space] : (TrackBreakpointType?)null
				})
				.ToList();
			DistanceFromShip = section.DistanceFromShip;
		}
	}
}
