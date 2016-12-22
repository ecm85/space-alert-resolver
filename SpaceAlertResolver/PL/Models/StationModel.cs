using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class StationModel
	{
		public IEnumerable<int> EnergyCubes { get; set; }
		public IEnumerable<PlayerModel> Players { get; set; }

		public StationModel(Game game, StationLocation station)
		{
			Players = GetPlayersInStation(game, station).Select(player => new PlayerModel(player)).ToList();
			EnergyCubes = Enumerable.Range(0, game.SittingDuck.StandardStationsByLocation[station].BravoComponent.EnergyInComponent);
		}

		private static IEnumerable<Player> GetPlayersInStation(Game game, StationLocation location)
		{
			return game.Players.Where(player => player.CurrentStation.StationLocation == location);
		}
	}
}
