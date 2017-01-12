using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class WhiteZoneModel : StandardZoneModel
	{
		public WhiteZoneModel(Game game) : base(game, ZoneLocation.White)
		{
			UpperStation = new UpperWhiteStationModel(game);
			LowerStation = new LowerWhiteStationModel(game);
		}
	}
}
