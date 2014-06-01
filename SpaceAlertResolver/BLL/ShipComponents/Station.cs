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
		public CComponent CComponent { get; set; }
		public ZoneLocation ZoneLocation { get; set; }
		public ISet<InternalThreat> Threats { get; private set; }
		public IList<Player> Players { get; private set; }

		public Station()
		{
			Players = new List<Player>();
			Threats = new HashSet<InternalThreat>();
		}

		public void PerformBAction(Player performingPlayer, int currentTurn)
		{
			var firstBThreat = GetFirstThreatOfType(PlayerAction.B);
			if (firstBThreat == null)
				EnergyContainer.PerformBAction();
			else
				DamageThreat(firstBThreat, performingPlayer);
		}

		public PlayerDamage PerformAAction(Player performingPlayer, int currentTurn)
		{
			var firstAThreat = GetFirstThreatOfType(PlayerAction.A);
			if (firstAThreat == null)
				return Cannon.PerformAAction();
			DamageThreat(firstAThreat, performingPlayer);
			return null;
		}

		public CResult PerformCAction(Player performingPlayer, int currentTurn)
		{
			var firstCThreat = GetFirstThreatOfType(PlayerAction.C);
			if (firstCThreat == null)
			{
				var cResult = CComponent.PerformCAction(performingPlayer);
				//TODO: Use cResult
				return cResult;
			}
			DamageThreat(firstCThreat, performingPlayer);
			return null;
		}

		public InternalPlayerDamageResult UseBattleBots(Player performingPlayer, int currentTurn)
		{
			var firstBattleBotThreat = GetFirstThreatOfType(PlayerAction.BattleBots);
			return firstBattleBotThreat == null ? null : DamageThreat(firstBattleBotThreat, performingPlayer);
		}

		private InternalThreat GetFirstThreatOfType(PlayerAction playerAction)
		{
			return Threats
				.Where(threat => threat.ActionType == playerAction)
				.OrderBy(threat => threat.TimeAppears)
				.FirstOrDefault();
		}

		private InternalPlayerDamageResult DamageThreat(InternalThreat threat, Player performingPlayer)
		{
			return threat.TakeDamage(1, performingPlayer);
			//TODO: Handle removing from track, removing from ship.CurrentList and scoring
		}
	}
}
