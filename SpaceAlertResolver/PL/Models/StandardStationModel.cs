using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public abstract class StandardStationModel : StationModel
	{
		public IEnumerable<int> EnergyCubes { get; set; }
		public int MaxEnergyCubes { get; set; }
		public CannonModel Cannon { get; set; }
		public IEnumerable<InternalThreatInZoneModel> InternalThreats { get; set; }

		protected StandardStationModel(Game game, StationLocation station) : base(game, station)
		{
			var standardStation = game.SittingDuck.StandardStationsByLocation[station];
			EnergyCubes = Enumerable.Range(1, standardStation.BravoComponent.EnergyInComponent);
			MaxEnergyCubes = standardStation.BravoComponent.Capacity + 1;
			Cannon = new CannonModel(standardStation.AlphaComponent);
			InternalThreats = game.SittingDuck.ThreatController.InternalThreatsOnTrack
				.Where(threat => threat.CurrentStations.Contains(station))
				.Select(threat => new InternalThreatInZoneModel(threat))
				.ToList();
		}
	}
}
