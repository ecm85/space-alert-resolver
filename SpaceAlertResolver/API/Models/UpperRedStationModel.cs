using BLL;
using BLL.ShipComponents;

namespace API.Models
{
	public class UpperRedStationModel : StandardStationModel
	{
		public InterceptorsModel Interceptors { get; set; }

		public UpperRedStationModel(Game game)
			: base(game, StationLocation.UpperRed)
		{
			if (game.SittingDuck.RedZone.UpperRedStation.InterceptorComponent.Interceptors != null)
				Interceptors = new InterceptorsModel();
		}
	}
}
