using BLL.Tracks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
	public class TrackSnapshotModel
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public TrackConfiguration TrackConfiguration { get; set; }

		public TrackSnapshotModel(Track track)
		{
			TrackConfiguration = track.TrackConfiguration;
		}
	}
}
