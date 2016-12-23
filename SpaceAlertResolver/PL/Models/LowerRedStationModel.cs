using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class LowerRedStationModel : StandardStationModel
	{
		public LowerRedStationModel(Game game) : base(game, StationLocation.LowerRed)
		{
		}
	}
}