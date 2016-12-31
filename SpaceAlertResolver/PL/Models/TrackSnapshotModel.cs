using System.Collections.Generic;
using System.Linq;
using BLL.Threats;
using BLL.Tracks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
	public class TrackSnapshotModel
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public TrackConfiguration Track { get; set; }
		public string DisplayName => Track.DisplayName();
		public IEnumerable<TrackSectionModel> Sections { get; set; }
	
		public TrackSnapshotModel(Track track, IEnumerable<Threat> threatsOnTrack)
		{
			Track = track.TrackConfiguration;
			Sections = track.Sections.Select(section => new TrackSectionModel(section, threatsOnTrack, track.Breakpoints)).ToList();
		}
	}
}
