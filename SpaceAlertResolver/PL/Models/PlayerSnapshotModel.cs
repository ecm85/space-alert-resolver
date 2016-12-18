using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class PlayerSnapshotModel
	{
		public int Index { get; set; }
		public StationLocation StationLocation { get; set; }
		public bool HasBattleBots { get; set; }

		public PlayerSnapshotModel(Player player)
		{
			Index = player.Index;
			StationLocation = player.CurrentStation.StationLocation;
			HasBattleBots = player.BattleBots != null;
		}
	}
}
