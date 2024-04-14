using BLL;
using BLL.ShipComponents;

namespace API.Models
{
	public class InterceptorsStationModel : StationModel
	{
		public InterceptorsStationModel(Game game, StationLocation station)
			: base(game, station) { }
	}
}
