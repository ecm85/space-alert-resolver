using BLL.Tracks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
	public class TrackSpaceModel
	{
		public int Space { get; set; }
		public bool HasAnyThreats { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public TrackBreakpointType? Breakpoint { get; set; }

		[JsonConstructor]
		public TrackSpaceModel()
		{
			
		}
	}
}