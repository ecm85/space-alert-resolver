using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class BlueZoneModel : ZoneModel
	{
		public BlueZoneModel(Game game) : base(game, ZoneLocation.Blue)
		{
			UpperStation = new UpperBlueStationModel(game);
			LowerStation = new LowerBlueStationModel(game);
		}
	}
}
