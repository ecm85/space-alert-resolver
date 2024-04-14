using System.Collections.Generic;
using BLL;
using BLL.Players;
using BLL.ShipComponents;

namespace API.Models
{
	public class InputModel
	{
		public IEnumerable<ActionModel> SingleActions { get; set; }
		public IEnumerable<ActionModel> DoubleActions { get; set; }
		public IEnumerable<PlayerSpecializationActionModel> SpecializationActions { get; set; }
		public IEnumerable<TrackSnapshotModel> Tracks { get; set; }
		public AllThreatsModel AllInternalThreats { get; set; }
		public AllThreatsModel AllExternalThreats { get; set; }
		public IEnumerable<PlayerSpecialization> PlayerSpecializations { get; set; }
		public IEnumerable<DamageToken> AllDamageTokens { get; set; }
		public IEnumerable<ZoneLocation> DamageableZones { get; set; }
	}
}
