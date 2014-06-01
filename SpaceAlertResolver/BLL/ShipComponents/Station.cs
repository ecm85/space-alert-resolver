using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats.Internal;

namespace BLL.ShipComponents
{
	public class Station : IStation
	{
		public Station RedwardStation { get; set; }
		public Station BluewardStation { get; set; }
		public Station OppositeDeckStation { get; set; }
		public EnergyContainer EnergyContainer { get; set; }
		public Cannon Cannon { get; set; }
		public CComponent CComponent { private get; set; }
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

		public void PerformCAction(Player performingPlayer, int currentTurn)
		{
			var firstCThreat = GetFirstThreatOfType(PlayerAction.C);
			if (firstCThreat == null)
				CComponent.PerformCAction(performingPlayer);
			else
				DamageThreat(firstCThreat, performingPlayer);
		}

		public void UseBattleBots(Player performingPlayer)
		{
			var firstBattleBotThreat = GetFirstThreatOfType(PlayerAction.BattleBots);
			if (firstBattleBotThreat != null)
				DamageThreat(firstBattleBotThreat, performingPlayer);
		}

		private InternalThreat GetFirstThreatOfType(PlayerAction playerAction)
		{
			return Threats
				.Where(threat => threat.ActionType == playerAction)
				.OrderBy(threat => threat.TimeAppears)
				.FirstOrDefault();
		}

		private void DamageThreat(InternalThreat threat, Player performingPlayer)
		{
			threat.TakeDamage(1, performingPlayer);
			if (threat.RemainingHealth <= 0)
			{
				//TODO: Handle removing from track, removing from ship.CurrentList and scoring
			}
		}

		public void PerformNoAction(Player performingPlayer)
		{
		}

		public void UseInterceptors(Player performingPlayer)
		{
		}
	}
}
