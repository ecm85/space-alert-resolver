using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class UpperBlueStationModel : StandardStationModel
	{
		public BattleBotsModel BattleBots { get; set; }
		public UpperBlueStationModel(Game game) : base(game, StationLocation.UpperBlue)
		{
			var battleBots = game.SittingDuck.BlueZone.UpperBlueStation.BattleBotsComponent.BattleBots;
			if (battleBots != null)
				BattleBots = new BattleBotsModel(battleBots);
		}
	}
}