using System.Collections.Generic;
using System.Linq;
using BLL.Threats;
using BLL.Tracks;

namespace PL.Models
{
	public class TrackSectionSnapshotModel
	{
		public int DistanceFromShip { get; set; }
		public IEnumerable<TrackSpaceModel> TrackSpaces { get; set; }
		public TrackSectionSnapshotModel(TrackSection section, IEnumerable<Threat> threatsOnTrack)
		{
			TrackSpaces = Enumerable.Range(1 + 5 * (section.DistanceFromShip - 1), section.Length)
				.Reverse()
				.Select(space => new TrackSpaceModel
				{
					Space = space,
					HasAnyThreats = threatsOnTrack.Any(threat => threat.Position == space)
				})
				.ToList();
			DistanceFromShip = section.DistanceFromShip;
		}
	}
}