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

		public void PerformBAction()
		{
			var firstBThreat = GetFirstThreatOfType(PlayerAction.B);
			if (firstBThreat == null)
				EnergyContainer.PerformBAction();
			else
				DamageThreat(firstBThreat);
		}

		public InternalPlayerDamageResult UseBattleBots()
		{
			var firstBattleBotThreat = GetFirstThreatOfType(PlayerAction.BattleBots);
			return firstBattleBotThreat == null ? null : firstBattleBotThreat.TakeDamage(1);
		}

		private InternalThreat GetFirstThreatOfType(PlayerAction playerAction)
		{
			return
				Threats
					.Where(threat => threat.ActionType == playerAction)
					.OrderBy(threat => threat.TimeAppears)
					.FirstOrDefault();
		}

		private void DamageThreat(InternalThreat threat)
		{
			threat.TakeDamage(1);
			//TODO: Handle killing it
		}
	}
}
