using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class UpperRedStationModel : StandardStationModel
	{
		public UpperRedStationModel(Game game) : base(game, StationLocation.UpperRed)
		{
		}
	}
}