using System.Collections.Generic;
using BLL;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
	public class InputModel
	{
		public IEnumerable<ActionModel> SingleActions { get; set; }
		public IEnumerable<ActionModel> DoubleActions { get; set; }
		public IEnumerable<PlayerSpecializationActionModel> SpecializationActions { get; set; }
		public IEnumerable<TrackSnapshotModel> Tracks { get; set; }
		public AllThreatsModel AllInternalThreats { get; set; }
		public AllThreatsModel AllExternalThreats { get; set; }
		[JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
		public IEnumerable<PlayerSpecialization> PlayerSpecializations { get; set; }
	}
}
