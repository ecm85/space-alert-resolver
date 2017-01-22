using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class LowerBlueStationModel : StandardStationModel
	{
		public IEnumerable<int> Rockets { get; set; }
		public bool BatteryPackHasEnergy { get; set; }
		public bool RocketFiredThisTurn { get; set; }
		public bool RocketFiredLastTurn { get; set; }

		public LowerBlueStationModel(Game game) : base(game, StationLocation.LowerBlue)
		{
			Rockets = Enumerable.Range(1, game.SittingDuck.BlueZone.LowerBlueStation.RocketsComponent.RocketCount);
			BatteryPackHasEnergy = game.SittingDuck.BlueZone.LowerBlueStation.BatteryPack.HasEnergy;
			RocketFiredLastTurn = game.SittingDuck.BlueZone.LowerBlueStation.RocketsComponent.RocketFiredLastTurn != null;
			RocketFiredThisTurn = game.SittingDuck.BlueZone.LowerBlueStation.RocketsComponent.RocketFiredThisTurn != null;
		}
	}
}
