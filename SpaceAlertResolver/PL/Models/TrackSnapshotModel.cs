using System.Linq;
using BLL.Tracks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
	public class TrackSnapshotModel
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public TrackConfiguration TrackConfiguration { get; set; }
		public int Length { get; set; }

		public TrackSnapshotModel(Track track)
		{
			TrackConfiguration = track.TrackConfiguration;
			Length = track.Sections.Sum(section => section.Length);
		}
	}
}
