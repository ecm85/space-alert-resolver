using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
    public class LowerRedStationModel : StandardStationModel
    {
        public BattleBotsModel BattleBots { get; set; }
        public bool BatteryPackHasEnergy { get; set; }

        public LowerRedStationModel(Game game) : base(game, StationLocation.LowerRed)
        {
            var battleBots = game.SittingDuck.RedZone.LowerRedStation.BattleBotsComponent.BattleBots;
            if (battleBots != null)
                BattleBots = new BattleBotsModel(battleBots);
            BatteryPackHasEnergy = game.SittingDuck.RedZone.LowerRedStation.BatteryPack.HasEnergy;
        }
    }
}
