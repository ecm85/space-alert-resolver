using System.Collections.Generic;
using System.Linq;
using BLL.Tracks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
	public class TrackSnapshotModel
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public TrackConfiguration Track { get; set; }
		public int TrackIndex { get; set; }
		public string DisplayName => Track.DisplayName();
		public IEnumerable<TrackSectionModel> Sections { get; set; }

		[JsonConstructor]
		public TrackSnapshotModel()
		{

		}

		public TrackSnapshotModel(Track track, IEnumerable<int> threatPositions)
		{
			Track = track.TrackConfiguration;
			TrackIndex = (int)track.TrackConfiguration;
			Sections = track.Sections.Select(section => new TrackSectionModel(section, threatPositions, track.Breakpoints)).ToList();
		}
	}
}
