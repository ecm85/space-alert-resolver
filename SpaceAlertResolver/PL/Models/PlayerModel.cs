using System.Collections.Generic;
using System.Linq;
using BLL.Players;
using BLL.ShipComponents;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
    public class PlayerModel
    {
        public int Index { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public StationLocation StationLocation { get; set; }
        public bool HasBattleBots { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PlayerColor PlayerColor { get; set; }
        public BattleBotsModel BattleBots { get; set; }
        public IEnumerable<ActionModel> Actions { get; set; }
        public InterceptorsModel Interceptors { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PlayerSpecialization? PlayerSpecialization { get; set; }

        [JsonConstructor]
        public PlayerModel()
        {
            
        }

        public PlayerModel(Player player)
        {
            Index = player.Index;
            StationLocation = player.CurrentStation.StationLocation;
            HasBattleBots = player.BattleBots != null;
            PlayerColor = player.PlayerColor;
            if (player.BattleBots != null)
                BattleBots = new BattleBotsModel(player.BattleBots);
            Actions = player.Actions.Select(action => ActionModelFactory.Create(action, player)).ToList();
            if (player.Interceptors != null)
                Interceptors = new InterceptorsModel();
            PlayerSpecialization = player.Specialization;

            //Uncomment this to get battle bots in turn 1 on the client.
            //else if (player.PlayerColor == PlayerColor.Red || player.PlayerColor == PlayerColor.Green)
            //    BattleBots = new BattleBotsModel(new BattleBots());
        }
    }
}
