using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public abstract class StandardStationModel
	{
		public IEnumerable<int> EnergyCubes { get; set; }
		public int MaxEnergyCubes { get; set; }
		public IEnumerable<PlayerModel> Players { get; set; }
		public CannonModel Cannon { get; set; }

		protected StandardStationModel(Game game, StationLocation station)
		{
			Players = GetPlayersInStation(game, station).Select(player => new PlayerModel(player)).ToList();
			var standardStation = game.SittingDuck.StandardStationsByLocation[station];
			EnergyCubes = Enumerable.Range(1, standardStation.BravoComponent.EnergyInComponent);
			MaxEnergyCubes = standardStation.BravoComponent.Capacity + 1;
			Cannon = new CannonModel(standardStation.AlphaComponent);
		}

		private static IEnumerable<Player> GetPlayersInStation(Game game, StationLocation location)
		{
			return game.Players.Where(player => player.CurrentStation.StationLocation == location);
		}
	}
}
