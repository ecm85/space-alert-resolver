using System.Collections.Generic;
using System.Linq;
using BLL.Tracks;

namespace PL.Models
{
	public class TrackSectionSnapshotModel
	{
		public int DistanceFromShip { get; set; }
		public IEnumerable<int> TrackSpaces { get; set; }
		public TrackSectionSnapshotModel(TrackSection section)
		{
			TrackSpaces = Enumerable.Range(1 + 5 * (section.DistanceFromShip - 1), section.Length).Reverse().ToList();
			DistanceFromShip = section.DistanceFromShip;
		}
	}
}