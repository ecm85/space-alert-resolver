using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats.Internal;

namespace BLL.ShipComponents
{
	public class Station
	{
		//TODO: Rename to left/right, at least here?
		public Station RedwardStation { get; set; }
		public Station BluewardStation { get; set; }
		public Station OppositeDeckStation { get; set; }
		public EnergyContainer EnergyContainer { get; set; }
		public Cannon Cannon { get; set; }
		public ZoneLocation ZoneLocation { get; set; }
		public ISet<InternalThreat> Threats { get; set; }

		public InternalPlayerDamageResult UseBattleBots()
		{
			var firstBattleBotThreat = Threats
				.Where(threat => threat.ActionType == PlayerAction.BattleBots)
				.OrderBy(threat => threat.TimeAppears)
				.FirstOrDefault();
			return firstBattleBotThreat == null ? null : firstBattleBotThreat.TakeDamage(1);
		}
	}
}
