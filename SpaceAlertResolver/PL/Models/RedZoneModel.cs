using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
    public class RedZoneModel : StandardZoneModel
    {
        public RedZoneModel(Game game) : base(game, ZoneLocation.Red)
        {
            UpperStation = new UpperRedStationModel(game);
            LowerStation = new LowerRedStationModel(game);
        }
    }
}
