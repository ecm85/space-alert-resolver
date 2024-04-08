using System.Text.Json.Serialization;
using BLL.ShipComponents;

namespace API.Models
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
