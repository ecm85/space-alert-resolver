using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
    public class LowerWhiteStationModel : StandardStationModel
    {
        public IEnumerable<int> FuelCapsules { get; set; }
        public LowerWhiteStationModel(Game game) : base(game, StationLocation.LowerWhite)
        {
            FuelCapsules = Enumerable.Range(1, game.SittingDuck.WhiteZone.LowerWhiteStation.CentralReactor.FuelCapsules);
        }
    }
}