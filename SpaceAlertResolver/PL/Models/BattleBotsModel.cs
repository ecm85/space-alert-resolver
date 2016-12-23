using BLL.ShipComponents;

namespace PL.Models
{
	public class BattleBotsModel
	{
		public bool IsDisabled { get; set; }
		public BattleBotsModel(BattleBots battleBots)
		{
			IsDisabled = battleBots.IsDisabled;
		}
	}
}
