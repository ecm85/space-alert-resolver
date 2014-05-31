using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats.Internal;

namespace BLL.ShipComponents
{
	public class Station : IStation
	{
		//TODO: Rename to left/right, at least here?
		public Station RedwardStation { get; set; }
		public Station BluewardStation { get; set; }
		public Station OppositeDeckStation { get; set; }
		public EnergyContainer EnergyContainer { get; set; }
		public Cannon Cannon { get; set; }
		public ZoneLocation ZoneLocation { get; set; }
		public ISet<InternalThreat> Threats { get; private set; }
		public IList<Player> Players { get; private set; }

		public Station()
		{
			Players = new List<Player>();
			Threats = new HashSet<InternalThreat>();
		}

		public void PerformBAction()
		{
			var firstBThreat = GetFirstThreatOfType(PlayerAction.B);
			if (firstBThreat == null)
				EnergyContainer.PerformBAction();
			else
				DamageThreat(firstBThreat);
		}

		public PlayerDamage PerformAAction()
		{
			var firstAThreat = GetFirstThreatOfType(PlayerAction.A);
			if (firstAThreat == null)
				return Cannon.PerformAAction();
			DamageThreat(firstAThreat);
			return null;
		}

		public void PerformCAction()
		{
			var firstCThreat = GetFirstThreatOfType(PlayerAction.C);
			if (firstCThreat == null)
				//TODO: Handle C Actions
				throw new NotImplementedException();
			DamageThreat(firstCThreat);
		}

		public InternalPlayerDamageResult UseBattleBots()
		{
			var firstBattleBotThreat = GetFirstThreatOfType(PlayerAction.BattleBots);
			return firstBattleBotThreat == null ? null : DamageThreat(firstBattleBotThreat);
		}

		private InternalThreat GetFirstThreatOfType(PlayerAction playerAction)
		{
			return Threats
				.Where(threat => threat.ActionType == playerAction)
				.OrderBy(threat => threat.TimeAppears)
				.FirstOrDefault();
		}

		private InternalPlayerDamageResult DamageThreat(InternalThreat threat)
		{
			return threat.TakeDamage(1);
			//TODO: Handle removing from track and scoring
		}
	}
}
