using BLL.ShipComponents;
using Newtonsoft.Json;

namespace PL.Models
{
	public class BattleBotsModel
	{
		public bool IsDisabled { get; set; }
		public BattleBotsModel(BattleBots battleBots)
		{
			IsDisabled = battleBots.IsDisabled;
		}

		[JsonConstructor]
		public BattleBotsModel()
		{
			
		}
	}
}
