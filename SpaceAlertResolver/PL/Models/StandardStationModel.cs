using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public abstract class StandardStationModel
	{
		public IEnumerable<int> EnergyCubes { get; set; }
		public IEnumerable<PlayerModel> Players { get; set; }

		protected StandardStationModel(Game game, StationLocation station)
		{
			Players = GetPlayersInStation(game, station).Select(player => new PlayerModel(player)).ToList();
			EnergyCubes = Enumerable.Range(1, game.SittingDuck.StandardStationsByLocation[station].BravoComponent.EnergyInComponent);
		}

		private static IEnumerable<Player> GetPlayersInStation(Game game, StationLocation location)
		{
			return game.Players.Where(player => player.CurrentStation.StationLocation == location);
		}
	}
}
