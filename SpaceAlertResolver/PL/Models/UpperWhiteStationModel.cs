using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class UpperWhiteStationModel : StandardStationModel
	{
		public UpperWhiteStationModel(Game game) : base(game, StationLocation.UpperWhite)
		{
		}
	}
}