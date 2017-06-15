using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
    public abstract class StationModel
    {
        public IEnumerable<PlayerModel> Players { get; set; }

        protected StationModel(Game game, StationLocation station)
        {
            Players = game.SittingDuck.StationsByLocation[station].Players.Select(player => new PlayerModel(player)).ToList();
        }
    }
}
