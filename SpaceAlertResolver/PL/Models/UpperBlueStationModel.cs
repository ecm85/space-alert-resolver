using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class UpperBlueStationModel : StandardStationModel
	{
		public UpperBlueStationModel(Game game) : base(game, StationLocation.UpperBlue)
		{
		}
	}
}