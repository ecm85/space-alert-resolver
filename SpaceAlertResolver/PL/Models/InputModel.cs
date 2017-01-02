using System.Collections.Generic;

namespace PL.Models
{
	public class InputModel
	{
		public IEnumerable<ActionModel> Actions { get; set; }
		public IEnumerable<TrackSnapshotModel> Tracks { get; set; }
		public AllThreatsModel AllInternalThreats { get; set; }
		public AllThreatsModel AllExternalThreats { get; set; }
	}
}
