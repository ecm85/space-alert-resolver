using System.Collections.Generic;
using System.Linq;
using BLL.Tracks;
using Newtonsoft.Json;

namespace PL.Models
{
	public class TrackSectionModel
	{
		public int DistanceFromShip { get; set; }
		public IEnumerable<TrackSpaceModel> TrackSpaces { get; set; }
		public TrackSectionModel(TrackSection section, IEnumerable<int> threatPositions, IDictionary<int, TrackBreakpointType> breakpoints)
		{
			TrackSpaces = Enumerable.Range(1 + 5 * (section.DistanceFromShip - 1), section.Length)
				.Reverse()
				.Select(space => new TrackSpaceModel
				{
					Space = space,
					HasAnyThreats = threatPositions.Contains(space),
					Breakpoint = breakpoints.ContainsKey(space) ? breakpoints[space] : (TrackBreakpointType?)null
				})
				.ToList();
			DistanceFromShip = section.DistanceFromShip;
		}

		[JsonConstructor]
		public TrackSectionModel()
		{
			
		}
	}
}
