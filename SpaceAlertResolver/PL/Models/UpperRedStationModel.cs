using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
    public class UpperRedStationModel : StandardStationModel
    {
        public InterceptorsModel Interceptors { get; set; }
        public UpperRedStationModel(Game game) : base(game, StationLocation.UpperRed)
        {
            if (game.SittingDuck.RedZone.UpperRedStation.InterceptorComponent.Interceptors != null)
                Interceptors = new InterceptorsModel();
        }
    }
}
