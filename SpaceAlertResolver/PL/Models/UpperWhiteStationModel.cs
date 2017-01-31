using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class UpperWhiteStationModel : StandardStationModel
	{
		public IEnumerable<int> ComputerCubes { get; set; }

		public UpperWhiteStationModel(Game game) : base(game, StationLocation.UpperWhite)
		{
			ComputerCubes = Enumerable.Range(1, game.SittingDuck.WhiteZone.UpperWhiteStation.ComputerComponent.RemainingComputerCheckTurns.Count);
		}
	}
}