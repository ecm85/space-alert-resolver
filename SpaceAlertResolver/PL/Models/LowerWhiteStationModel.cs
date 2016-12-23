using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class LowerWhiteStationModel : StandardStationModel
	{
		public LowerWhiteStationModel(Game game) : base(game, StationLocation.LowerWhite)
		{
		}
	}
}