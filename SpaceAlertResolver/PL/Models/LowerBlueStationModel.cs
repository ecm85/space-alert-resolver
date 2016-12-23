using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class LowerBlueStationModel : StandardStationModel
	{
		public IEnumerable<int> Rockets { get; set; }

		public LowerBlueStationModel(Game game) : base(game, StationLocation.LowerBlue)
		{
			Rockets = Enumerable.Range(1, game.SittingDuck.BlueZone.LowerBlueStation.RocketsComponent.RocketCount);
		}
	}
}
