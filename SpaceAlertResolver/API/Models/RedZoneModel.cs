using BLL;
using BLL.ShipComponents;

namespace API.Models
{
	public class RedZoneModel : StandardZoneModel
	{
		public RedZoneModel(Game game)
			: base(game, ZoneLocation.Red)
		{
			UpperStation = new UpperRedStationModel(game);
			LowerStation = new LowerRedStationModel(game);
		}
	}
}
